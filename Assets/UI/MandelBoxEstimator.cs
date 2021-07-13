using System;
using ProjectionScreen;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class MandelBoxEstimator:MonoBehaviour
    {
        private DistanceEstimatorPanel distanceEstimatorPanel;

        public TracerRenderer tracer;

        void Start()
        {
            distanceEstimatorPanel = GetComponent<DistanceEstimatorPanel>();
        }

        private float De(Vector3 pos)
        {
            var tscale = tracer.Scale;
            float iterations = 16;
            var minRad2 = tracer.MinRad2;
            
            var absScalem1 = Math.Abs(tscale - 1.0f);
            var absScaleRaisedTo1MIters = Mathf.Pow(Math.Abs(tscale), (1.0f-iterations));
            Vector4 p = new Vector4(pos.x,pos.y,pos.z,1.0f), p0 = p;  // p.w is the distance estimate
            var scale = new Vector4(tscale, tscale, tscale, Math.Abs(tscale)) / minRad2;
            for (var i=0; i<iterations; i++)
            {
                
                var tmp = Clamp(Xyz(p), -Vector3.one, Vector3.one) * 2.0f - Xyz(p);  // min;max;mad
                p.x = tmp.x;
                p.y = tmp.y;
                p.z = tmp.z;
                var r2 = Vector3.Dot(Xyz(p),Xyz(p));
                //if (i<ColorIterations) orbitTrap = min(orbitTrap, abs(vec4(p.Xyz,r2)));
                
                p *= Mathf.Clamp(Mathf.Max(minRad2/r2, minRad2), 0.0f, 1.0f);  // dp3,div,max.sat,mul
                p = new Vector4(p.x * scale.x, p.y * scale.y, p.z * scale.z, p.w * scale.w) + p0;
                if ( r2>1000.0f) break;
		
            }

            var d = ((Xyz(p).magnitude - absScalem1) / p.w - absScaleRaisedTo1MIters);
            return d;
        }


        private static Vector3 Xyz(Vector4 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
        private static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max)
        {
            return new Vector3(Mathf.Clamp(v.x,min.x,max.x),
                Mathf.Clamp(v.y,min.y,max.y),
                Mathf.Clamp(v.z,min.z,max.z));
        }

        private float Distance(Ray r)
        {
            var t = 0f;
            var i = 0;
            while (i < 16)
            {
                var p = r.origin + r.direction * t;
                var d = De(p);
                if (d < 0.01f)
                {
                    return t;
                }

                t += d;
                i++;
            }

            return -1f;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //create a ray cast and set it to the mouses cursor position in game
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var d = Distance(ray);
                if (d > 0.0f)
                {
                    distanceEstimatorPanel.ShowBrackets(Input.mousePosition,d);
                }
            }
        }
    }
    
}