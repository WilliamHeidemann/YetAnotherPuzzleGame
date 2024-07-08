using Model;
using Systems;
using UnityEngine;

namespace Components
{
    public class GroundBlock : MonoBehaviour, IInteractable
    {
        public Location location;
        public void Interact()
        {
            Selector.Instance.TryMoveTo(location);
        }
    }
}