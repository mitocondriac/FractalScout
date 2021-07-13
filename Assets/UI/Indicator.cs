using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Indicator : MonoBehaviour
    {
        public Text DistanceData;

        public void SetText(float dist)
        {
            var dd = dist.ToString();
            
            DistanceData.text = $"Dist: {dd.Substring(0,Math.Min(dd.Length,5))}";
        }
    }
}