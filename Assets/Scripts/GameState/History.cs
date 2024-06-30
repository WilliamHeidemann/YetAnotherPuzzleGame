using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace GameState
{
    public class History
    {
        private readonly Dictionary<Type, LinkedList<Move>> moveStacks = new();
        private readonly List<Move> allMoves = new();

        public History()
        {
            var types = For.GetValues<Type>();
            foreach (var type in types)
            {
                moveStacks.Add(type, new LinkedList<Move>());
            }
        }

        public void Add(Move move)
        {
            allMoves.Add(move);
            if (!move.isUndo)
                moveStacks[move.type].AddLast(move);
        }

        public Option<Move> GetLastMove(Type type)
        {
            if (moveStacks[type].Count <= 0) 
                return Option<Move>.None;
            
            var last = moveStacks[type].Last.Value;
            return Option<Move>.Some(last);
        }

        public void Undo(Move move)
        {
            moveStacks[move.type].Remove(move);
        }

        public Option<Move> GetMove(Location next)
        {
            return allMoves.LastOption(m => m.next == next);
        }
        
        public int Count(Type type) => moveStacks[type].Count(m => m.type == type);
        public int count => moveStacks.Values.Sum(moves => moves.Count);
    }
}