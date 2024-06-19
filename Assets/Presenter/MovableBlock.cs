using UnityEngine;

namespace Presenter
{
    public class MovableBlock : MonoBehaviour
    {
        public Model.Block model;
        private static MovableBlock hovered;

        private void OnMouseEnter()
        {
            if (hovered == this) return;
            hovered = this;
            if (!Level.Instance.HasMoves()) return;
            Level.Instance.ShowGhostBlocks(model);
            CommandManager.Instance.Hover(this);
        }

        public static void NullifyHovered() => hovered = null;
    }
}