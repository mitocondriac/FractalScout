uniform float Scale;
uniform float MinRad2;
uniform int Iterations;

float de(float3 pos, inout float3 orbit) {
	const float absScalem1 = abs(Scale - 1.0);
	const float absScaleRaisedTo1MIters = pow(abs(Scale), 1.0 - Iterations);
	float4 p = float4(pos.x, pos.y, pos.z, 1.0);  // p.w is the distance estimate
	const float4 p0 = p;
	const float4 scale = float4(Scale, Scale, Scale, abs(Scale)) / MinRad2;
	float xf = 0.0;
	float oLen = 1000000.0;
	for (int i = 0; i < Iterations; i++)
	{
		const float3 tmp = clamp(p.xyz, -float3(1, 1, 1), float3(1, 1, 1)) * 2.0 - p.xyz;  // min;max;mad
		p.xyz = tmp.xyz;

		const float r2 = dot(p.xyz, p.xyz);
		xf += r2;
		p *= clamp(max(MinRad2 / r2, MinRad2), 0.0, 1.0);  // dp3,div,max.sat,mul
		p = float4(p.x * scale.x, p.y * scale.y, p.z * scale.z, p.w * scale.w) + p0;
		float ol = length(orbit);
		if (ol<oLen)
		{
			orbit = abs(p);
			oLen=ol;
		}
		if (r2 > 1000.0) break;

	}

	return (length(p.xyz) - absScalem1) / p.w - absScaleRaisedTo1MIters;

}

float de(float3 pos) {
	const float absScalem1 = abs(Scale - 1.0);
	const float absScaleRaisedTo1MIters = pow(abs(Scale), 1.0 - Iterations);
	float4 p = float4(pos.x, pos.y, pos.z, 1.0);  // p.w is the distance estimate
	const float4 p0 = p;
	const float4 scale = float4(Scale, Scale, Scale, abs(Scale)) / MinRad2;
	float xf = 0.0;
	for (int i = 0; i < Iterations; i++)
	{
		const float3 tmp = clamp(p.xyz, -float3(1, 1, 1), float3(1, 1, 1)) * 2.0 - p.xyz;  // min;max;mad
		p.xyz = tmp.xyz;

		const float r2 = dot(p.xyz, p.xyz);
		xf += r2;
		p *= clamp(max(MinRad2 / r2, MinRad2), 0.0, 1.0);  // dp3,div,max.sat,mul
		p = float4(p.x * scale.x, p.y * scale.y, p.z * scale.z, p.w * scale.w) + p0;

		if (r2 > 1000.0) break;

	}

	return (length(p.xyz) - absScalem1) / p.w - absScaleRaisedTo1MIters;

}