using System;
using Model;
using Systems;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Components
{
    public class MovableBlock : MonoBehaviour, IInteractable
    {
        public Block model;
        private Option<UndoTimer> undoTimer = Option<UndoTimer>.None;
        
        // On Interact
        // Check if we have a timer running (this means the block is already selected)
        // If we dont, select this block and start a timer
        // If we do, check if the timer has finished. If it has not finished, we have double clicked
        // In this case, undo, and set the timer to none

        public void Interact()
        {
            if (undoTimer.IsSome(out var timer) && !timer.isFinished)
            {
                Selector.Instance.Undo();
                undoTimer = Option<UndoTimer>.None;
                return;
            }

            Controller.Instance.Select(this);
            undoTimer = Option<UndoTimer>.Some(new UndoTimer());
        }
        
        // Old hold to undo logic:
        // public void Interact()
        // {
        //     Controller.Instance.Select(this);
        //     undoTimer = Option<UndoTimer>.Some(new UndoTimer());
        // }
        //
        // private void Update()
        // {
        //     if (!undoTimer.IsSome(out var timer)) return;
        //     if (!timer.isFinished) return;
        //     Selector.Instance.Undo();
        //     undoTimer = Option<UndoTimer>.None;
        // }
        //
        // private void OnMouseUpAsButton()
        // {
        //     if (!undoTimer.IsSome(out var timer)) return;
        //     undoTimer = Option<UndoTimer>.None;
        // }

        private class UndoTimer
        {
            public bool isFinished => timeOfCreation + TimeToFinish < Time.time;
            public float percentDone => (Time.time - timeOfCreation) / TimeToFinish * 100;
            private const float TimeToFinish = 0.5f;
            private readonly float timeOfCreation;
            public UndoTimer()
            {
                timeOfCreation = Time.time;
            }
        }
    }
}