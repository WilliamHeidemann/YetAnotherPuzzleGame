using System;
using UnityEngine;

namespace Components
{
    [RequireComponent(typeof(Light))]
    public class Highlight : MonoBehaviour
    {
        [SerializeField] private Light spotlight;
        [SerializeField] private float timeToShine;

        private void Start()
        {
            LeanTween.value(gameObject, SetSpotAngle, 0f, 100f, timeToShine).setEase(LeanTweenType.easeOutQuint);
        }
        
        void SetSpotAngle(float value)
        {
            spotlight.spotAngle = value;
        }
    }
}