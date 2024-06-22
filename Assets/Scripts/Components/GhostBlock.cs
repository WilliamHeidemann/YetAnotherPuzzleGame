using Model;
using Presenter;
using Systems;
using UnityEngine;

namespace Components
{
    public class GhostBlock : MonoBehaviour
    {
        public Block model;
        private void OnMouseDown()
        {
            MoveSelector.Instance.Select(model);
        }

        private void Start()
        {
            LeanTween.move(gameObject, model.location.asVector3, 1f).setEase(LeanTweenType.easeOutExpo);
        }
    }
}