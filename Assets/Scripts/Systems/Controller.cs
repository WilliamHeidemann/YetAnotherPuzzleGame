using System.Linq;
using Components;
using GameState;
using Model;
using UnityEngine;
using UnityUtils;
using Grid = GameState.Grid;
using Type = Model.Type;

namespace Systems
{
    public class Controller : Singleton<Controller>
    {
        [SerializeField] private Spawner spawner;
        [SerializeField] private MoveSelector moveSelector;
        private Grid grid;
        private History history;
        private LevelManager levelManager;
        private MoveCounter moveCounter;
        public bool HasMoves() => moveCounter.hasMovesLeft && levelManager.isLevelComplete == false;

        public void TryMove(Move move)
        {
            if (!grid.IsMoveValid(move))
                return; // Should always be valid, as ghost blocks only spawn in valid positions
            if (levelManager.isLevelComplete)
                return;
            if (!moveCounter.hasMovesLeft)
                return; // Should always have moves left, as ghost blocks only spawn with moves left

            moveCounter.IncrementCount();
            Move(move);
        }

        public void UndoCardinal() => TryUndo(Type.Cardinal);
        public void UndoDiagonal() => TryUndo(Type.Diagonal);

        private void TryUndo(Type type)
        {
            if (!history.HasUndo(type, out var previousMove))
                return;
            if (levelManager.isLevelComplete)
                return;
            var undoMove = new Move(previousMove.next, previousMove.previous, type);
            if (!grid.IsMoveValid(undoMove))
                return;

            moveCounter.DecrementCount();
            Move(undoMove);
        }

        private void Move(Move move)
        {
            spawner.HideGhostBlocks();
            grid.Move(move);
            history.Undo(move.type);
            moveSelector.Move(move);
            MovableBlock.NullifyHovered();
            levelManager.CheckCompletion(grid.GetBlocks());
        }
    }
}