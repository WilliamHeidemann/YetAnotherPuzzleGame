using System;
using Model;
using Systems;
using UnityEngine;

namespace Components
{
    public class MovableBlock : MonoBehaviour, IInteractable
    {
        public Block model;
        public void Interact()
        {
            Controller.Instance.Select(this);
        }
    }
}