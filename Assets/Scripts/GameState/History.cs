using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Type = Model.Type;

namespace GameState
{
    public class History
    {
        public int count => doneCardinalCommands.Count + doneDiagonalCommands.Count;
        public int previousCardinalCount => doneCardinalCommands.Count;
        public int previousDiagonalCount => doneDiagonalCommands.Count;

        // Maybe just have a dictionary mapping every type to a stack?
        private readonly Stack<Move> doneCardinalCommands = new();
        private readonly Stack<Move> doneDiagonalCommands = new();

        private Stack<Move> Previous(Type type) =>
            type switch
            {
                Type.Cardinal => doneCardinalCommands,
                Type.Diagonal => doneDiagonalCommands,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

        public void Add(Move move)
        {
            Previous(move.type).Push(move);
        }

        public bool HasUndo(Type type, out Move move)
        {
            var moves = Previous(type);
            moves.TryPeek(out move);
            return moves.Any();
        }

        public void Undo(Type type)
        {
            if (!HasUndo(type, out var _)) return;
            Previous(type).Pop();
        }
    }
}