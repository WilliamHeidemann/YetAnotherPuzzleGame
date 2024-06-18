using System;
using Model;
using UnityEngine;

namespace Presenter
{
    public class GhostBlock : MonoBehaviour
    {
        public Model.Block model;

        private void OnMouseDown()
        {
            CommandManager.Instance.Select(model);
        }
    }
}