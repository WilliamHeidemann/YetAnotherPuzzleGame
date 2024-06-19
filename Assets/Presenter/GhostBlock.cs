using System;
using Model;
using UnityEngine;

namespace Presenter
{
    public class GhostBlock : MonoBehaviour
    {
        public Model.Block model;
        public Model.Block origin;
        private void OnMouseDown()
        {
            Block.NullifyHovered();
            CommandManager.Instance.Select(model);
        }

        private void Start()
        {
            LeanTween.move(gameObject, model.location.asVector3, 1f).setEase(LeanTweenType.easeOutExpo);
        }
    }
}