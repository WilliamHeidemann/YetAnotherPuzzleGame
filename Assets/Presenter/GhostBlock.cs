using System;
using Model;
using UnityEngine;

namespace Presenter
{
    public class GhostBlock : MonoBehaviour
    {
        public Block model;
        public Block origin;
        private void OnMouseDown()
        {
            CommandManager.Instance.Select(model);
        }

        private void Start()
        {
            LeanTween.move(gameObject, model.location.asVector3, 1f).setEase(LeanTweenType.easeOutExpo);
        }
    }
}