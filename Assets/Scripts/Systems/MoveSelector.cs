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

        public void Hover(Block block)
        {
            selected = block;
        }

        public void Select(Location destination)
        {
            var command = new Move(selected.location, destination, selected.type);
            Controller.Instance.TryMove(command);
        }

        public void Move(Move move)
        {
            var blockToMove = FindObjectsByType<MovableBlock>(FindObjectsSortMode.None)
                .First(block => block.model.location == move.previous);
            blockToMove.model = new Block(move.next, move.type);
            Controller.Instance.CheckCompletion();
            var targetLocation = move.next.asVector3;
            MoveAnimator.Move(blockToMove.gameObject, targetLocation);
        }
    }
}