using System.Collections.Generic;
using System.Linq;
using Components;
using Model;
using Presenter;
using UnityEngine;
using UnityUtils;

namespace Systems
{
    public class MoveSelector : Singleton<MoveSelector>
    {
        private Block toMove;

        public void Hover(MovableBlock block)
        {
            toMove = block.model;
        }

        public void Select(Block toLand)
        {
            Level.Instance.HideGhostBlocks();
            var command = new Move(toMove, toLand, toMove.type);
            Level.Instance.TryMove(command);
        }

        public void Do(Move move) => Move(move.previous, move.next);

        public void Undo(Move move) => Move(move.next, move.previous);

        private void Move(Block from, Block to)
        {
            var blockToMove = FindObjectsByType<MovableBlock>(FindObjectsSortMode.None)
                .First(block => block.model.location == from.location);
            blockToMove.model = to;
            Level.Instance.CheckCompletion();
            var targetLocation = to.location.asVector3;
            MoveAnimator.Tween(blockToMove.gameObject, targetLocation);
        }
    }
}