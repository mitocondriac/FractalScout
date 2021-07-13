using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LogPanel : MonoBehaviour
    {
        // Start is called before the first frame update
        public Navigator Nav;
        public Text LogText;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            LogText.text = Nav.GetStats();
        }
    }
}