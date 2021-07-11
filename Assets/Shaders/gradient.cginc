
#include "de.cginc"

float3 gradient(float3 pos, float normalDistance)
{
	normalDistance = max(normalDistance * 0.5, 1.0e-7);
	const float3 ex = float3(normalDistance, 0.0, 0.0);
	const float3 ey = float3(0.0, normalDistance, 0.0);
	const float3 ez = float3(0.0, 0.0, normalDistance);
	const float3 n = float3(de(pos + ex) - de(pos - ex),
	                        de(pos + ey) - de(pos - ey),
	                        de(pos + ez) - de(pos - ez));
	return normalize(n);
}
