float3 getSphere(float3 pos, float3 dir)
{
	float t = 0;
	int i = 0;
	while (i < 32) {
		float3 p = pos + dir * t;
		float d = length(p) - 1.0;

		d *= 0.9;
		if (d < 0.01) {
			float c = clamp(-dot(normalize(p), dir), 0, 1);
			return float3(1, c, c);
		}
		t += d;
		i++;
	}
	return float3(0.5, 0.5, 1);
}