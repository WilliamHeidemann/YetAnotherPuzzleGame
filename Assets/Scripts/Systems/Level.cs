using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using UnityEngine;
using Model;
using ScriptableObjects;
using Systems;
using UnityUtils;
using UtilityToolkit.Runtime;
using Grid = Model.Grid;
using Type = Model.Type;

namespace Presenter
{
    public class Level : Singleton<Level>
    {

        private Grid grid;
        private History history;
        private MoveData moveData;
        private bool isLevelComplete;


        

        public bool HasMoves() => moveData.used.Value < moveData.max && isLevelComplete == false;


        public void TryMove(Move move)
        {
            if (!grid.IsMoveValid(move)) return;
            history.Add(move);
            grid.Move(move);
            MoveSelector.Instance.Do(move);
            moveData.IncrementCount();
            MovableBlock.NullifyHovered();
        }

        private void RedoCommand(Type type)
        {
            if (!history.HasRedo(type, out var command)) return;
            if (!grid.IsMoveValid(command)) return;
            HideGhostBlocks();
            history.Redo(type);
            grid.Move(command);
            MoveSelector.Instance.Do(command);
            moveData.IncrementCount();
            MovableBlock.NullifyHovered();
        }

        public void UndoCardinalCommand() => UndoCommand(Type.Cardinal);
        public void UndoDiagonalCommand() => UndoCommand(Type.Diagonal);

        private void UndoCommand(Type type)
        {
            if (!history.HasUndo(type, out var command)) return;
            if (!grid.IsUndoMoveValid(command)) return;
            HideGhostBlocks();
            history.Undo(type);
            grid.Move(command);
            MoveSelector.Instance.Undo(command);
            moveData.DecrementCount();
            MovableBlock.NullifyHovered();
        }

        public void CheckCompletion()
        {
            var board = grid.GetBlocks();
            var goal = currentLevel.targetConfiguration;
            isLevelComplete = board.All(block => goal.Contains(block));
            if (isLevelComplete) NextLevel();
        }
    }
}