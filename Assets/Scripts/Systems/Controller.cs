using System;
using System.Linq;
using Components;
using GameState;
using Model;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityUtils;
using Grid = GameState.Grid;
using Type = Model.Type;

namespace Systems
{
    public class Controller : Singleton<Controller>
    {
        [SerializeField] private TextMeshProUGUI moveCounterText;
        [SerializeField] private Spawner spawner;
        [SerializeField] private MoveSelector moveSelector;
        private Grid grid;
        private History history;
        private LevelManager levelManager;
        private MoveCounter moveCounter;

        protected override void Awake()
        {
            base.Awake();
            MovableBlock.OnHover += BlockHover;
        }

        private void Update()
        {
            Animator.HandleAnimations();
        }

        private bool HasMoves() => moveCounter.hasMovesLeft && levelManager.isLevelComplete == false;

        public void Initialize(Level level, LevelManager manager)
        {
            spawner.SpawnLevel(level);
            grid = new Grid(level.width, level.height, level.startingConfiguration);
            history = new History();
            levelManager = manager;
            moveCounter = new MoveCounter(level.maxMoves, moveCounterText);
        }

        private void BlockHover(Block block)
        {
            if (!HasMoves())
                return;
            spawner.ShowGhostBlocks(block, grid.IsAvailable);
            moveSelector.Select(block);
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

            moveCounter.IncrementCount();
            Move(move);
        }

        public void UndoCardinal() => TryUndo(Type.Cardinal);
        public void UndoDiagonal() => TryUndo(Type.Diagonal);

        private void TryUndo(Type type)
        {
            if (levelManager.isLevelComplete)
                return;
            
            if (!history.HasUndo(type, out var previousMove))
                return;
            
            var undoMove = new Move(previousMove.next, previousMove.previous, type);
            if (!grid.IsMoveValid(undoMove))
                return;

            moveCounter.DecrementCount();
            Move(undoMove, isUndo: true);
        }

        private void Move(Move move, bool isUndo = false)
        {
            var block = spawner.GetMovableBlock(move.previous);
            if (!block.IsSome(out var blockToMove))
                throw new Exception($"Block not found at {move.previous}. (Should not be possible)");
            blockToMove.model.location = move.next;

            var targetLocation = move.next.asVector3;
            Animator.Move(blockToMove.gameObject, targetLocation, move.type);

            spawner.HideGhostBlocks();
            grid.Move(move);
            if (!isUndo)
                history.Add(move);
            
            MovableBlock.NullifyHovered();
            levelManager.CheckCompletion(grid.GetBlocks());
        }
    }
}