using System.Linq;
using Components;
using GameState;
using Model;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityUtils;
using Animator = Animation.Animator;
using Grid = GameState.Grid;

namespace Systems
{
    public class Controller : Singleton<Controller>
    {
        private Grid grid;
        private History history;
        private MoveCounter moveCounter;
        private bool isLevelComplete;
        
        public void Initialize(Level level)
        {
            Spawner.Instance.SpawnLevel(level);
            ResetGameState(level);
        }

        private void ResetGameState(Level level)
        {
            grid = new Grid(level.startingConfiguration, level.groundBlocks);
            history = new History();
            moveCounter = new MoveCounter(level.maxMoves);
            isLevelComplete = false;
            Select(Spawner.Instance.getFirstBlock);
        }

        public void Select(MovableBlock movable)
        {
            Selector.Instance.Select(movable);

            if (isLevelComplete)
            {
                Spawner.Instance.HideHighlights();
                return;
            }
            UpdateUndoButton(movable);

            var block = movable.model;
            var validNeighbors = block.GetAvailableNeighbors(grid.HasBlockAt, grid.HasGroundAt).ToList();

            if (validNeighbors.Count > 0 && moveCounter.hasMovesLeft)
            {
                Spawner.Instance.ShowHighlights(validNeighbors);
            }
            else if (history.GetPreviousLocation(movable).IsSome(out var previousLocation))
            { 
                var list = validNeighbors.Where(l => l == previousLocation).ToList();
                Spawner.Instance.ShowHighlights(list);
            }
            else
            {
                Spawner.Instance.HideHighlights();
                Animator.Shake(movable.gameObject);
            }
        }

        public void TryUndo(MovableBlock movable)
        {
            if (!history.GetPreviousLocation(movable).IsSome(out var previous))
                return;

            var move = new Move(movable.model.location, previous, movable.model.type, true);
            TryMove(move);
        }

        public void TryMove(Move move)
        {
            if (isLevelComplete)
                return;
            if (!grid.IsMoveValid(move))
                return;

            Move(move);
        }

        public void Rewind() => Rewind(LevelManager.Instance.currentLevel);

        private async void Rewind(Level level)
        {
            ResetGameState(level);
            await Spawner.Instance.ResetLevel(level);
        }

        private void Move(Move move)
        {
            var block = Spawner.Instance.GetMovableBlock(move.previous);
            if (!block.IsSome(out var blockToMove))
                return;

            var isUndo = history.GetPreviousLocation(blockToMove).IsSome(out var previousLocation) &&
                         previousLocation == move.next;
            if (isUndo)
            {
                moveCounter.DecrementCount();
                history.Undo(blockToMove);
            }
            else if (moveCounter.hasMovesLeft)
            {
                moveCounter.IncrementCount();
                history.Add(blockToMove, move.previous);
            }
            else return; 

            grid.Move(move);

            blockToMove.model = blockToMove.model.WithLocation(move.next);
            var targetLocation = move.next.asVector3.With(y: -0.35f);
            Animator.Move(blockToMove.gameObject, targetLocation, move.type, true);

            Spawner.Instance.HideHighlights();

            Select(blockToMove);

            isLevelComplete =
                LevelManager.Instance.currentLevel.targetConfiguration.TrueForAll(grid.GetBlocks().Contains);
            if (isLevelComplete)
                DelayedEnterNextLevel();
        }

        private async void DelayedEnterNextLevel()
        {
            await Awaitable.WaitForSecondsAsync(Animator.MoveTime);
            LevelManager.Instance.LevelComplete();
        }

        public void UpdateUndoButton(MovableBlock block)
        {
            if (history.GetPreviousLocation(block).IsSome(out var previous) && 
                grid.IsAvailable(previous))
                UndoButton.Instance.Enable(block.model.type);
            else 
                UndoButton.Instance.Disable();
        }
    }
}