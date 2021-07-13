using UnityEngine;

namespace UI
{
    public class DistanceEstimatorPanel : MonoBehaviour
    {        
        public Indicator Track;
        private GameObject _track;
        private float _timer;
        void Start()
        {
            _track = Track.gameObject;
            _track.SetActive(false);

        }

        void Update()
        {
            if (_timer < 0f) return;
            if (Time.fixedTime - _timer > 2f)
            {
                _track.SetActive(false);
                _timer = -1f;
            }
        }
        public void ShowBrackets(Vector3 mousePosition, float d)
        {
            _track.SetActive(true);
            _timer = Time.fixedTime;
            _track.transform.position = mousePosition;
            Track.SetText(d);
        }

    }
}