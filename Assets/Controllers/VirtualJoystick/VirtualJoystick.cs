using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Controllers.VirtualJoystick
{
    public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector2 _lastMousePosition;
        private Vector3 _startPosition;
        private float _maxrange = 25f;
        private float _resettimer;
        public bool LockVertical;
        public OnControllerMove OnControllerMoveEvent = new OnControllerMove();
        public Vector2 CurrentPosition = new Vector2();
        public Vector2 DeltaMovement = new Vector2();
        [Tooltip("Seconds delay before reset. Set to 0 if you don't want to reset.")]

        public float ResetTimer;
        void Start()
        {
            _startPosition = GetComponent<RectTransform>().localPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("Begin Drag");
            _lastMousePosition = eventData.position;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 currentMousePosition = eventData.position;
            Vector2 diff = currentMousePosition - _lastMousePosition;
            RectTransform rect = GetComponent<RectTransform>();

            Vector3 newPosition = Clamp(rect.localPosition + new Vector3(diff.x, diff.y, rect.localPosition.z));
            Vector3 oldPos = rect.localPosition;
            if (LockVertical) newPosition.y = rect.localPosition.y;
            rect.localPosition = newPosition;
            if (!IsRectTransformInsideSreen(rect))
            {
                rect.position = oldPos;
            }
            _lastMousePosition = currentMousePosition;

            var cp = new Vector2(newPosition.x - _startPosition.x, newPosition.y - _startPosition.y)/ _maxrange;
            DeltaMovement = cp - CurrentPosition;
            CurrentPosition = cp;
            OnControllerMoveEvent.Invoke(CurrentPosition);
        }


        private Vector3 Clamp(Vector3 p)
        {
            var pxy = new Vector2(p.x, p.y);
            var spxy = new Vector2(_startPosition.x, _startPosition.y);
            float nd = Mathf.Clamp((pxy - spxy).magnitude, 0f, _maxrange);
            Vector2 npd = (pxy-spxy).normalized;
            var newp = npd * nd + spxy;
            return new Vector3(newp.x, newp.y, _startPosition.z);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("End Drag");
            _resettimer = Time.time;
            //Implement your funtionlity here
        }

        private bool IsRectTransformInsideSreen(RectTransform rectTransform)
        {
            bool isInside = false;
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            int visibleCorners = 0;
            Rect rect = new Rect(0, 0, Screen.width, Screen.height);
            foreach (Vector3 corner in corners)
            {
                if (rect.Contains(corner))
                {
                    visibleCorners++;
                }
            }
            if (visibleCorners == 4)
            {
                isInside = true;
            }
            return isInside;
        }

        void Update()
        {

            if (ResetTimer>0f && Time.time - _resettimer > ResetTimer)
            {
                var rect = GetComponent<RectTransform>();
                rect.localPosition = _startPosition;
                CurrentPosition = new Vector2();
                DeltaMovement = new Vector2();
                _resettimer = float.MaxValue;
                OnControllerMoveEvent.Invoke(CurrentPosition);

            }
        }
    }
    public class OnControllerMove:UnityEvent<Vector2>{}
}