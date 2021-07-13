using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class RudderController : MonoBehaviour
    {
        public float RudderResetTimer;
        private float _resettimer;

        private Slider Rudder;

        void Start()
        {
            _resettimer = float.MaxValue;
            Rudder = GetComponent<Slider>();
        }

        public void StartTimer()
        {
            _resettimer = Time.time;
        }
        void Update()
        {

            if (RudderResetTimer>0f && Time.time - _resettimer > RudderResetTimer)
            {
                Debug.Log("Resetting");
                Rudder.value = 0f;
                _resettimer = float.MaxValue;

            }
        }
    }
}