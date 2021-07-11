using UnityEngine;

namespace ProjectionScreen
{
    public class TracerRenderer : MonoBehaviour
    {
        // Start is called before the first frame update
        public Material Mat;
        public GameObject Light;
        public GameObject SpotLight;
        public Color LightColor;
        public Color MinColor;
        public Color MaxColor;
        public Color SkyColor;
        public Color GlowColor;
        public Color DiffuseLightColor;
        public float FogIntensity;
        public float Threshold;
        public float MinRad2;
        public float Scale;
        public float MaxDistance;
        public float GlowMax;
        public float OcclusionEffect;
        public int Iterations;
        public int TracerIterations;
        public Navigator Nav;
        private Camera m_c;

        void Start()
        {
            m_c = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            var r00 = m_c.ViewportPointToRay(new Vector3(0, 0, 0)).direction;
            var r01= m_c.ViewportPointToRay(new Vector3(0, 1, 0)).direction;
            var r10= m_c.ViewportPointToRay(new Vector3(1, 0, 0)).direction;
            var r11= m_c.ViewportPointToRay(new Vector3(1, 1, 0)).direction;
            var position = m_c.transform.position;
            Debug.DrawRay(position, r00*100f,Color.green,100f);
            Debug.DrawRay(position, r01 * 100f, Color.green,100f);
            Debug.DrawRay(position, r10 * 100f, Color.green,100f);
            Debug.DrawRay(position, r11 * 100f, Color.green,100f);
            Mat.SetVector("R00", r00);
            Mat.SetVector("R01", r01);
            Mat.SetVector("R10", r10);
            Mat.SetVector("R11", r11);
            Mat.SetVector("CamDir", m_c.transform.forward);
            Mat.SetVector("CamPos", position);

            Mat.SetFloat("MinRad2",MinRad2);
            Mat.SetFloat("Scale",Scale);
            Mat.SetFloat("Threshold", Threshold);
            Mat.SetInteger("Iterations", Iterations);
            Mat.SetInteger("TracerIterations", TracerIterations);
            Mat.SetVector("LightPos", Light.transform.position);
            Mat.SetVector("LightColor", LightColor);
            Mat.SetVector("MinColor", MinColor);
            Mat.SetVector("MaxColor", MaxColor);
            Mat.SetVector("SkyColor", SkyColor);
            Mat.SetVector("GlowColor", GlowColor);
            Mat.SetVector("DiffuseLightColor", DiffuseLightColor);
            Mat.SetVector("SpotLight", SpotLight.transform.position);
            Mat.SetFloat("FogIntensity", FogIntensity);
            Mat.SetFloat("MaxDistance", MaxDistance);
            Mat.SetFloat("OcclusionEffect", OcclusionEffect);
            Mat.SetFloat("GlowMax", GlowMax);

        }
    }
}
