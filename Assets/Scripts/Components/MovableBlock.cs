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
        public void Interact()
        {
            Controller.Instance.Select(this);
            undoTimer = Option<UndoTimer>.Some(new UndoTimer());
        }

        private void Update()
        {
            if (!undoTimer.IsSome(out var timer)) return;
            if (!timer.isFinished) return;
            Selector.Instance.Undo();
            undoTimer = Option<UndoTimer>.None;
        }

        private void OnMouseUpAsButton()
        {
            if (!undoTimer.IsSome(out var timer)) return;
            undoTimer = Option<UndoTimer>.None;
        }

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