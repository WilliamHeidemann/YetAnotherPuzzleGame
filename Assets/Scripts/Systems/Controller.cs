using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using GameState;
using Model;
using NUnit.Framework;
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
        [SerializeField] private Selector selector;
        private Grid grid;
        private History history;
        private MoveCounter moveCounter;
        private bool isLevelComplete;

        private bool HasMoves() => moveCounter.hasMovesLeft && isLevelComplete == false;

        public void Initialize(Level level)
        {
            Spawner.Instance.SpawnLevel(level);
            ResetGameState(level);
        }

        private void ResetGameState(Level level)
        {
            grid = new Grid(level.startingConfiguration, level.groundBlocks);
            history = new History();
            moveCounter = new MoveCounter(level.maxMoves, moveCounterText);
            isLevelComplete = false;
            Select(Spawner.Instance.getFirstBlock);
        }

        public void Select(MovableBlock movable)
        {
            selector.Select(movable);

            if (isLevelComplete)
            {
                Spawner.Instance.HideHighlights();
                return;
            }

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

        public void TryMove(Move move)
        {
            if (isLevelComplete)
                return;
            if (!grid.IsMoveValid(move))
                return;
            // if (!moveCounter.hasMovesLeft)
            //     return;

            Move(move);
        }

        public void Rewind() => Rewind(LevelManager.Instance.currentLevel);

        private async void Rewind(Level level)
        {
            ResetGameState(level);
            await Spawner.Instance.ResetLevel(level);
        }

        // public void TryUndo(Block block)
        // {
        //     if (isLevelComplete)
        //         return;
        //
        //     if (!history.GetMove(block).IsSome(out var move))
        //         return;
        //
        //     if (!grid.IsAvailable(move.previous))
        //     {
        //         // Currently the player is not given the option to undo, if it is not valid. Otherwise, 
        //         // Play animation showing which way is undo, and that something is in the way
        //         return;
        //     }
        //
        //     history.Undo(block, move);
        //     Move(move.reversed);
        // }


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
            Animator.Move(blockToMove.gameObject, targetLocation, move.type);

            Spawner.Instance.HideHighlights();
            
            
            // Change the following 5 lines
            // the history should be used to figure out if the move is an undo move,
            // in which case the user can get back their movement point 
            
            // If it is, undo it in history
            // If it is not, record the move
            // history.Add(blockToMove.model, move);
            
            // Controller might also need history to know where to display undo ghost block

            

            Select(blockToMove);
            
            isLevelComplete =
                LevelManager.Instance.currentLevel.targetConfiguration.TrueForAll(grid.GetBlocks().Contains);
            if (isLevelComplete)
                DelayedEnterNextLevel();
        }

        // private bool BlockCanUndo(Block block)
        // {
            // if (!history.GetMove(block).IsSome(out var move)) return false;
            // if (move.isUndo) return false;
            // if (!grid.IsAvailable(move.previous)) return false;
            // return true;
        // }

        private async void DelayedEnterNextLevel()
        {
            await Awaitable.WaitForSecondsAsync(Animator.MoveTime);
            LevelManager.Instance.LevelComplete();
        }
    }
}