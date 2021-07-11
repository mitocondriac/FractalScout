Shader "screen"
{
	Properties
	{		
		_NoiseTex("NoiseTex", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;

		};

		uniform float4 R00;
		uniform float4 R01;
		uniform float4 R10;
		uniform float4 R11;
		uniform float4 CamDir;
		uniform float4 CamPos;
		uniform int TracerIterations;
		uniform float3 LightPos;
		uniform float3 LightColor;
		uniform float3 MaxColor;
		uniform float3 MinColor;
		uniform float3 GlowColor;
		uniform float3 SkyColor;
		uniform float3 DiffuseLightColor;
		uniform float3 SpotLight;
		uniform float FogIntensity;
		uniform float Threshold;
		uniform float MaxDistance;
		uniform float GlowMax;
		uniform float OcclusionEffect;

#include "random.cginc"
#include "gradient.cginc"
#include "fog.cginc"


		float3 getLight(float3 pos, float3 lightpos, float3 normal) {
			float3 empty = float3(0, 0, 0);

			const float3 dir = normalize(lightpos - pos);
			const float dist = length(lightpos - pos);
			float t = Threshold*10.0;
			int i = 0;
			while (i < TracerIterations/2.0) {
				const float3 p = pos + dir * t;
				const float d = de(p, empty);
				if (d < Threshold) {
					return float3(0,0,0);
				}
				if (t > dist) break;
				t += d;
				i++;
			}					

			return LightColor * dot(dir, normal);
		}

		float3 getmaterial(float3 dir, float3 normal, float3 orbit) {

			const float c = clamp(-dot(dir, normal), 0, 1);
			return lerp(MinColor, MaxColor, length(orbit))*c;
		}

		float3 trace(float3 pos, float3 dir)
		{
			float3 orbit = float3(1, 1, 1);
			float t = 0;
			float occ = 0.0;
			float inv = 1.0/float(TracerIterations);
			float3 p;
			float fSteps = 0;
			for (int i = 0;i<TracerIterations;i++) {
				p = pos + dir * t;

				const float d = de(p, orbit);
				fSteps += t/(d);

				if (d < Threshold) {
					p = pos + dir * (t - Threshold*0.5);
					const float3 n = gradient(p, Threshold);
					const float3 material = getmaterial(dir, n, orbit);
					const float3 l1 = getLight(p, LightPos, n) ;
					const float3 l2 = getLight(p, SpotLight, n) ;
					const float3 glow = clamp(GlowColor*GlowMax*fSteps/MaxDistance, float3(0,0,0),float3(1,1,1));
					float3 col = (DiffuseLightColor*clamp(1.0-occ*OcclusionEffect,float3(0,0,0),float3(1,1,1)) + l1 + l2) * material;
					return lerp(clamp(col+glow,float3(0,0,0),float3(1,1,1)), FogAmount(pos,p)*(SkyColor), 1.0-exp(-(t)*FogIntensity));
				}
				t += d;
				occ += d*i*inv/t;
				i++;
				if (t > MaxDistance) break;
			}
			const float3 glow = clamp(GlowColor*GlowMax*fSteps/MaxDistance, float3(0,0,0),float3(1,1,1));
			//return clamp(FogAmount(pos,p)*SkyColor + glow,float3(0,0,0),float3(1,1,1));
			return lerp(clamp(glow,0,1), FogAmount(pos,p)*SkyColor, 1.0-exp(-(t)*FogIntensity));



		}

		void surf(Input i , inout SurfaceOutputStandard o )
		{
			const float3 raydir = lerp(lerp(R00.xyz, R10.xyz, i.uv_texcoord.x), lerp(R01.xyz, R11.xyz, i.uv_texcoord.x), i.uv_texcoord.y);
			o.Albedo = trace(CamPos, raydir);
			o.Alpha = 1;
		}
		
		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
