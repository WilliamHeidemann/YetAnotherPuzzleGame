using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace GameState
{
    public class History
    {
        private readonly Dictionary<Block, List<Move>> allMoves = new();

        public void Add(Block block, Move move)
        {
            if (!allMoves.ContainsKey(block))
                allMoves.Add(block, new List<Move>());
            allMoves[block].Add(move);
        }

        public void Undo(Block block, Move move)
        {
            var index = allMoves[block].FindLastIndex(m => m == move);
            allMoves[block].RemoveAt(index);
        }

        public Option<Move> GetMove(Block block) =>
            !allMoves.TryGetValue(block, out var move) 
                ? Option<Move>.None 
                : move.LastOption(m => m.next == block.location && !m.isUndo);
    }
}