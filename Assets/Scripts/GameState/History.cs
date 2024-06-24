using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Type = Model.Type;

namespace GameState
{
    public class History
    {
        public int count => cardinalMoves.Count + diagonalMoves.Count;
        public int cardinalCount => cardinalMoves.Count;
        public int diagonalCount => diagonalMoves.Count;

        // Maybe just have a dictionary mapping every type to a stack?
        private readonly Stack<Move> cardinalMoves = new();
        private readonly Stack<Move> diagonalMoves = new();

        private Stack<Move> GetMoveStack(Type type) =>
            type switch
            {
                Type.Cardinal => cardinalMoves,
                Type.Diagonal => diagonalMoves,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

        public void Add(Move move)
        {
            GetMoveStack(move.type).Push(move);
        }

        public bool HasUndo(Type type, out Move move)
        {
            var moves = GetMoveStack(type);
            moves.TryPeek(out move);
            return moves.Any();
        }

        public void Undo(Type type)
        {
            if (!HasUndo(type, out var _)) return;
            GetMoveStack(type).Pop();
        }
    }
}