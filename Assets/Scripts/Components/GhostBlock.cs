using Model;
using Systems;
using UnityEngine;

namespace Components
{
    public class GhostBlock : MonoBehaviour, IInteractable
    {
        public Location location;

        private void Start()
        {
            LeanTween.move(gameObject, location.asVector3, 1f).setEase(LeanTweenType.easeOutExpo);
        }
        public void Interact()
        {
            Selector.Instance.MoveTo(location);
        }
    }
}