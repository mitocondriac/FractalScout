		uniform sampler2D _NoiseTex;
		float random(float2 i)
		{
			const float3 rr= tex2D(_NoiseTex,i).rgb;
			return clamp((rr.x+rr.y+rr.z)%1.0,0,1);
		}
		float random(float3 i)
		{
			const float3 rr1= tex2D(_NoiseTex,i.xy).rgb;
			const float3 rr2= tex2D(_NoiseTex,i.xz).rgb;
			const float3 rr3= tex2D(_NoiseTex,i.yz).rgb;
			return (clamp((rr1.x+rr1.y+rr1.z)%1.0,0,1)+
					clamp((rr2.x+rr2.y+rr2.z)%1.0,0,1)+
					clamp((rr3.x+rr3.y+rr3.z)%1.0,0,1))/3.0;
		}
