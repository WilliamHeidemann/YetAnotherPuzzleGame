using System.Collections.Generic;
using System.Linq;
using Components;
using Model;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace GameState
{
    public class History
    {
        // private readonly Dictionary<Block, List<Move>> allMoves = new();
        //
        // public void Add(Block block, Move move)
        // {
        //     if (!allMoves.ContainsKey(block))
        //         allMoves.Add(block, new List<Move>());
        //     allMoves[block].Add(move);
        // }
        //
        // public void Undo(Block block, Move move)
        // {
        //     var index = allMoves[block].FindLastIndex(m => m == move);
        //     allMoves[block].RemoveAt(index);
        // }
        //
        // public Option<Move> GetMove(Block block) =>
        //     !allMoves.TryGetValue(block, out var move) 
        //         ? Option<Move>.None 
        //         : move.LastOption(m => m.next == block.location && !m.isUndo);

        private readonly Dictionary<MovableBlock, Stack<Location>> previousLocations = new();

        // called whenever a block is moved somewhere new
        public void Add(MovableBlock block, Location previousLocation)
        {
            Debug.Log("Adding");
            if (!previousLocations.TryGetValue(block, out var stack))
            {
                stack = new Stack<Location>();
                previousLocations.Add(block, stack);
            }
            
            stack.Push(previousLocation);
        }

        public Option<Location> GetPreviousLocation(MovableBlock block)
        {
            if (!previousLocations.TryGetValue(block, out var stack))
                return Option<Location>.None;

            if (!stack.TryPeek(out var location))
                return Option<Location>.None;
            
            return Option<Location>.Some(location);
        }
        
        public void Undo(MovableBlock block)
        {
            if (!previousLocations.TryGetValue(block, out var stack))
                return;

            Debug.Log(stack.Count);
            stack.Pop();
        }
    }
}