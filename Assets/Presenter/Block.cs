using UnityEngine;

namespace Presenter
{
    public class Block : MonoBehaviour
    {
        public Model.Block model;
        private static Block hovered;

        private void OnMouseEnter()
        {
            if (hovered == this) return;
            hovered = this;
            if (!Level.Instance.HasMoves()) return;
            Level.Instance.ShowGhostBlocks(model);
            CommandManager.Instance.Hover(this);
        }
    }
}