using System;
using Model;
using Systems;
using UnityEngine;

namespace Components
{
    public class MovableBlock : MonoBehaviour
    {
        public Block model;
        private static MovableBlock hovered;

        private void OnMouseDown()
        {
            if (hovered == this) return;
            hovered = this;
            OnHover?.Invoke(model);
        }

        public static event Action<Block> OnHover;
        public static void NullifyHovered() => hovered = null;
    }
}