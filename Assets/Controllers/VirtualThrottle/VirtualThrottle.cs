using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers.VirtualThrottle
{
    public class VirtualThrottle : MonoBehaviour, IPointerClickHandler
    {
        [Tooltip("reverse is always half of max throttle at most")]

        public int MaxThrottle;

        public int Throttle = 0;
        private float _step;
        // Start is called before the first frame update
        void Start()
        {
            _step = 180f / MaxThrottle;
        }


        // Update is called once per frame
        void Update()
        {

        
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var oldThrottle = Throttle;
            RectTransform rect = GetComponent<RectTransform>();
            
            var pos = eventData.position;
            var rawpos = new Vector2(transform.position.x, transform.position.y);
            var np = 2f*(pos - rawpos) / rect.sizeDelta.x;
            var degrees= 180f-Mathf.Atan2(np.y, np.x) * 180f / Mathf.PI;
            if (degrees > 180f && degrees < 270f) return;
            if (degrees > 269.9f) degrees -= 360f;
            Throttle = Mathf.RoundToInt(degrees / _step);

            rect.Rotate(0f,0f,-(Throttle-oldThrottle)*_step);

            //Debug.Log(Mathf.RoundToInt(degrees/_step));
        }
    }
}
