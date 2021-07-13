using UnityEngine;

namespace UI
{
    public class AttitudeIndicator : MonoBehaviour
    {
        public GameObject Indicator;
        public GameObject Ship;

        void Update()
        {
            var ss = Ship.transform.eulerAngles;
            ss.z = -ss.z;
            ss.y = -ss.y;
            Indicator.transform.localEulerAngles = ss;
        }
    }
}