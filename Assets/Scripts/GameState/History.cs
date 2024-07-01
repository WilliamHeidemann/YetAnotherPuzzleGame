using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace GameState
{
    public class History
    {
        private readonly List<Move> allMoves = new();

        public void Add(Move move)
        {
            allMoves.Add(move);
        }

        public void Undo(Move move)
        {
            var index = allMoves.FindLastIndex(m => m == move);
            allMoves.RemoveAt(index);
        }

        public Option<Move> GetMove(Location next)
        {
            return allMoves.LastOption(m => m.next == next && !m.isUndo);
        }

        public int count => allMoves.Count;
    }
}