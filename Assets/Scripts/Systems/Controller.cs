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
using Grid = GameState.Grid;
using Type = Model.Type;

namespace Systems
{
    public class Controller : Singleton<Controller>
    {
        [SerializeField] private TextMeshProUGUI moveCounterText;
        [SerializeField] private Image undoCardinal;
        [SerializeField] private Image undoDiagonal;
        [SerializeField] private Spawner spawner;
        [SerializeField] private MoveSelector moveSelector;
        private Grid grid;
        private History2 history;
        private LevelManager levelManager;
        private MoveCounter moveCounter;

        protected override void Awake()
        {
            base.Awake();
            MovableBlock.OnHover += BlockHover;
        }

        private bool HasMoves() => moveCounter.hasMovesLeft && levelManager.isLevelComplete == false;

        public async void Initialize(Level level, LevelManager manager)
        {
            await spawner.SpawnLevel(level);
            grid = new Grid(level.width, level.height, level.startingConfiguration);
            history = new History2();
            levelManager = manager;
            moveCounter = new MoveCounter(level.maxMoves, moveCounterText);
            UpdateButtons();
        }

        private void BlockHover(Block block)
        {
            if (!HasMoves())
                return;

            if (block.neighbors.Any(grid.IsAvailable))
            {
                spawner.ShowGhostBlocks(block, grid.IsAvailable);
                moveSelector.Select(block);
            }
            else
            {
                var blockOption = spawner.GetMovableBlock(block.location);
                if (blockOption.IsSome(out var movableBlock))
                {
                    Animator.Shake(movableBlock.gameObject);
                }
            }
        }

        public void TryMove(Move move)
        {
            if (levelManager.isLevelComplete)
                return;
            if (!grid.IsMoveValid(move))
                throw new Exception(
                    $"Move is not valid {move.previous} -> {move.next} ({move.type}). (Should not be possible)");
            if (!moveCounter.hasMovesLeft)
                throw new Exception($"Move attempted when out of moves. (Should not be possible)");

            Move(move);
        }

        public void Rewind()
        {
            For.Each<Type>(Rewind);
        }

        private void Rewind(Type type)
        {
            var count = 0;
            while (history.Count(type) > 0)
            {
                TryUndo(type);
                count++;
                if (count == 10)
                {
                    Debug.Log("Limit reached");
                    break;
                }
            }
        }

        public void UndoCardinal() => TryUndo(Type.Cardinal);
        public void UndoDiagonal() => TryUndo(Type.Diagonal);

        private void TryUndo(Type type)
        {
            if (levelManager.isLevelComplete)
                return;

            if (!history.GetLastMove(type).IsSome(out var previousMove))
                return;

            ChainUndo(previousMove);
        }

        private void ChainUndo(Move move)
        {
            var moveThatLedHereOption = history.GetMove(move.previous);
            if (moveThatLedHereOption.IsSome(out var moveThatLedHere) &&
                !grid.IsAvailable(move.previous))
            {
                ChainUndo(moveThatLedHere);
            }

            history.Undo(move);
            Move(move.reversed);
        }

        private void Move(Move move)
        {
            var block = spawner.GetMovableBlock(move.previous);
            if (block.IsSome(out var blockToMove))
            {
                blockToMove.model.location = move.next;
                var targetLocation = move.next.asVector3;
                Animator.Move(blockToMove.gameObject, targetLocation, move.type);
            }

            spawner.HideGhostBlocks();
            grid.Move(move);
            history.Add(move);
            if (move.isUndo)
                moveCounter.DecrementCount();
            else
                moveCounter.IncrementCount();

            MovableBlock.NullifyHovered();
            levelManager.CheckCompletion(grid.GetBlocks());

            UpdateButtons();
        }

        private void UpdateButtons()
        {
            undoCardinal.color = undoCardinal.color.SetAlpha(history.Count(Type.Cardinal) == 0 ? 0.2f : 1f);
            undoDiagonal.color = undoDiagonal.color.SetAlpha(history.Count(Type.Diagonal) == 0 ? 0.2f : 1f);
        }
    }
}