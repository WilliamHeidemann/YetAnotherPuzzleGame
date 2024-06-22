using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class History
    {
        public int count => doneCardinalCommands.Count + doneDiagonalCommands.Count;
        public int doneCardinalCount => doneCardinalCommands.Count;
        public int doneDiagonalCount => doneDiagonalCommands.Count;

        // Maybe just have a dictionary mapping every type to a stack?
        private readonly Stack<Move> doneCardinalCommands = new();
        private readonly Stack<Move> doneDiagonalCommands = new();

        private Stack<Move> Done(Type type) =>
            type switch
            {
                Type.Cardinal => doneCardinalCommands,
                Type.Diagonal => doneDiagonalCommands,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        
        public void Add(Move move)
        {
            Done(move.type).Push(move);
        }

        public bool HasUndo(Type type, out Move move)
        {
            var commands = Done(type);
            commands.TryPeek(out move);
            return commands.Any();
        }

        public void Undo(Type type)
        {
            if (!HasUndo(type, out var command)) return;
            Done(type).Pop();
        }
    }
}