using UnityEngine;

namespace Components
{
    public class EmptyInteractable : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            print("Nothing!");
        }
    }
}