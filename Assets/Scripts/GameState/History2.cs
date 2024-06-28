using System.Collections.Generic;
using Model;
using UtilityToolkit.Runtime;

namespace GameState
{
    public class History2
    {
        private readonly List<Move> moves = new();

        public void Add(Move move)
        {
            moves.Add(move);
        }
        
        public bool HasUndo(Type type, out Move move)
        {
            var moveOption = moves.LastOption(m => m.type == type);
            return moveOption.IsSome(out move);
        }
        
        public void Undo(Move move)
        {
            moves.Remove(move);
        }
    }
}