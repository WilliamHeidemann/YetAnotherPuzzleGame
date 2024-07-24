using System;
using Systems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Components
{
    public class InteractionController : MonoBehaviour
    {
        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryInteract();
            }
            else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                TrySwipe();
            }

            
        }

        private void TryInteract()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
                hit.collider.GetComponent<IInteractable>()?.Interact();
            else
                Selector.Instance.Deselect();
        }
        
        private void TrySwipe()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;
            var interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable is GroundBlock groundBlock)
            {
                groundBlock.Interact();
            }
        }
    }
}