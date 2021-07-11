#include <UnityShaderVariables.cginc>
//=======================================================================================
float DefiniteIntegral (in float x, in float amplitude, in float frequency, in float motionFactor)
{
    // Fog density on an axis:
    // (1 + sin(x*F)) * A
    //
    // indefinite integral:
    // (x - cos(F * x)/F) * A
    //
    // ... plus a constant (but when subtracting, the constant disappears)
    //
    x += _Time.x * motionFactor;
    return (x - cos(frequency * x)/ frequency) * amplitude;
}
 
//=======================================================================================
float AreaUnderCurveUnitLength (in float a, in float b, in float amplitude, in float frequency, in float motionFactor)
{
    // we calculate the definite integral at a and b and get the area under the curve
    // but we are only doing it on one axis, so the "width" of our area bounding shape is
    // not correct.  So, we divide it by the length from a to b so that the area is as
    // if the length is 1 (normalized... also this has the effect of making sure it's positive
    // so it works from left OR right viewing).  The caller can then multiply the shape
    // by the actual length of the ray in the fog to "stretch" it across the ray like it
    // really is.
    return (DefiniteIntegral(a, amplitude, frequency, motionFactor) - DefiniteIntegral(b, amplitude, frequency, motionFactor)) / (a - b);
}
 
//=======================================================================================
float FogAmount (in float3 src, in float3 dest)
{
    float len = length(dest - src);
     
    // calculate base fog amount (constant density over distance)   
    float amount = len * 0.1;
     
    // calculate definite integrals across axes to get moving fog adjustments
    float adjust = 0.0;
    adjust += AreaUnderCurveUnitLength(dest.x, src.x, 0.01, 0.6, 2.0);
    adjust += AreaUnderCurveUnitLength(dest.y, src.y, 0.01, 1.2, 1.4);
    adjust += AreaUnderCurveUnitLength(dest.z, src.z, 0.01, 0.9, 2.2);
    adjust *= len;
     
    // make sure and not go over 1 for fog amount!
    return clamp(amount+adjust,0.0, 1.0);
}