using System.Collections.Generic;
using System.Linq;
using Components;
using Model;
using UnityEngine;
using UnityUtils;

namespace Systems
{
    public class MoveSelector : Singleton<MoveSelector>
    {
        private Block selected;

        public void Select(Block block)
        {
            selected = block;
        }

        public void Confirm(Location destination)
        {
            var command = new Move(selected.location, destination, selected.type);
            Controller.Instance.TryMove(command);
        }
    }
}