﻿using Model;
using Systems;
using UnityEngine;

namespace Components
{
    public class GhostBlock : MonoBehaviour
    {
        public Location location;
        private void OnMouseDown()
        {
            Selector.Instance.MoveTo(location);
        }

        private void Start()
        {
            LeanTween.move(gameObject, location.asVector3, 1f).setEase(LeanTweenType.easeOutExpo);
        }
    }
}