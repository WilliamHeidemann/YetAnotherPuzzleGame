using System;
using Model;
using Systems;
using UnityEngine;

namespace Components
{
    public class MovableBlock : MonoBehaviour
    {
        public Block model;

        private void OnMouseDown()
        {
            Controller.Instance.Select(this);
        }
    }
}