using Model;
using Systems;
using UnityEngine;

namespace Components
{
    public class MovableBlock : MonoBehaviour
    {
        public Block model;
        private static MovableBlock hovered;

        private void OnMouseEnter()
        {
            if (hovered == this) return;
            hovered = this;
            if (!Controller.Instance.HasMoves()) return;
            Controller.Instance.ShowGhostBlocks(model);
            MoveSelector.Instance.Hover(model);
        }

        public static void NullifyHovered() => hovered = null;
    }
}