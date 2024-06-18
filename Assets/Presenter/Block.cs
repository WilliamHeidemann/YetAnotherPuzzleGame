using UnityEngine;

namespace Presenter
{
    public class Block : MonoBehaviour
    {
        public Model.Block model;

        private void OnMouseEnter()
        {
            if (!Level.Instance.HasMoves()) return;
            Level.Instance.ShowGhostBlocks(model);
            CommandManager.Instance.Hover(this);
        }
    }
}