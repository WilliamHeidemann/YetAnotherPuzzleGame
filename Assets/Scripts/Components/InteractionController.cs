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
            if (!Input.GetMouseButtonDown(0))
                return;

            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
                hit.collider.GetComponent<IInteractable>()?.Interact();
            else
                Selector.Instance.Deselect();
        }
    }
}