using System;
using System.Linq;
using Components;
using GameState;
using Model;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils;
using UtilityToolkit.Runtime;
using Animator = Animation.Animator;
using Grid = GameState.Grid;
using Type = Model.Type;

namespace Systems
{
    public class Controller : Singleton<Controller>
    {
        [SerializeField] private TextMeshProUGUI moveCounterText;
        [SerializeField] private Spawner spawner;
        [SerializeField] private Selector selector;
        private Grid grid;
        private History history;
        private LevelManager levelManager;
        private MoveCounter moveCounter;

        private bool HasMoves() => moveCounter.hasMovesLeft && levelManager.isLevelComplete == false;

        public async void Initialize(Level level, LevelManager manager)
        {
            levelManager = manager;
            ResetGameState(level);
            await spawner.SpawnLevel(level);
        }

        private void ResetGameState(Level level)
        {
            grid = new Grid(level.startingConfiguration, level.groundBlocks);
            history = new History();
            moveCounter = new MoveCounter(level.maxMoves, moveCounterText);
            selector.Deselect();
            UndoButton.Instance.SetActive(false);
        }

        public void Select(MovableBlock movable)
        {
            selector.Select(movable);
            UndoButton.Instance.SetActive(BlockCanUndo(movable.model));
            spawner.HideHighlights();

            if (!HasMoves())
                return;

            var block = movable.model;
            var validNeighbors = block.GetAvailableNeighbors(grid.HasBlockAt, grid.HasGroundAt).ToList();

            if (validNeighbors.Count > 0)
            {
                spawner.ShowHighlights(validNeighbors);
            }
            else
            {
                Animator.Shake(movable.gameObject);
            }
        }

        public void TryMove(Move move)
        {
            if (levelManager.isLevelComplete)
                return;
            if (!grid.IsMoveValid(move))
                return;
            if (!moveCounter.hasMovesLeft)
                return;

            Move(move);
        }

        public void Rewind() => Rewind(levelManager.current);

        private async void Rewind(Level level)
        {
            ResetGameState(levelManager.current);
            await spawner.ResetLevel(level);
        }

        public void TryUndo(Block block)
        {
            if (levelManager.isLevelComplete)
                return;

            if (!history.GetMove(block).IsSome(out var move))
                return;

            if (!grid.IsAvailable(move.previous))
            {
                // Currently the player is not given the option to undo, if it is not valid. Otherwise, 
                // Play animation showing which way is undo, and that something is in the way
                return;
            }

            history.Undo(block, move);
            Move(move.reversed);
        }


        private void Move(Move move)
        {
            var block = spawner.GetMovableBlock(move.previous);
            if (!block.IsSome(out var blockToMove))
                return;

            blockToMove.model = blockToMove.model.WithLocation(move.next);
            var targetLocation = move.next.asVector3;
            Animator.Move(blockToMove.gameObject, targetLocation, move.type);

            spawner.HideHighlights();
            grid.Move(move);
            history.Add(blockToMove.model, move);
            UndoButton.Instance.SetActive(BlockCanUndo(blockToMove.model));

            if (move.isUndo)
                moveCounter.DecrementCount();
            else
                moveCounter.IncrementCount();

            levelManager.CheckCompletion(grid.GetBlocks());
        }

        private bool BlockCanUndo(Block block)
        {
            if (!history.GetMove(block).IsSome(out var move)) return false;
            if (move.isUndo) return false;
            if (!grid.IsAvailable(move.previous)) return false;
            return true;
        }
    }
}