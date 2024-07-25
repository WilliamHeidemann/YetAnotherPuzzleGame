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

        public void Interact()
        {
            Controller.Instance.Select(this);
            SoundEffectSystem.Instance.PlayWood();
            Handheld.Vibrate();
        }

        private class UndoTimer
        {
            public bool isFinished => timeOfCreation + timeToFinish < Time.time;
            public float fractionDone => (Time.time - timeOfCreation) / timeToFinish;
            private readonly float timeToFinish;
            private readonly float timeOfCreation = Time.time;

            public UndoTimer(float timeToFinish)
            {
                this.timeToFinish = timeToFinish;
            }
        }
    }
}