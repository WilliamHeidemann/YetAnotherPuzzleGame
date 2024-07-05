﻿using Systems;
using UnityEngine;
using Animator = Animation.Animator;

namespace Components
{
    public class RewindButton : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Controller.Instance.Rewind();
            Animator.Squish(gameObject);
        }
    }
}