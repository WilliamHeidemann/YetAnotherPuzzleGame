using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace GameState
{
    public class History2
    {
        private readonly Dictionary<Type, Stack<Move>> moveStacks = new();
        private readonly List<Move> allMoves = new();

        public History2()
        {
            var types = For.GetValues<Type>();
            foreach (var type in types)
            {
                moveStacks.Add(type, new Stack<Move>());
            }
        }

        public void Add(Move move)
        {
            allMoves.Add(move);
            if (!move.isUndo)
                moveStacks[move.type].Push(move);
        }

        public Option<Move> GetLastMove(Type type)
        {
            return moveStacks[type].Count > 0 ? Option<Move>.Some(moveStacks[type].Pop()) : Option<Move>.None;
        }

        public Option<Move> GetMove(Location next)
        {
            return allMoves.LastOption(m => m.next == next);
        }
        
        public int Count(Type type) => allMoves.Count(m => m.type == type);
    }
}