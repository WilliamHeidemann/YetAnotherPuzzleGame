using Systems;
using UnityEngine;

namespace Components
{
    public class RewindButton : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Controller.Instance.Rewind();
        }
    }
}