using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;

namespace Commands
{
    public class History
    {
        public int count => doneCardinalCommands.Count + doneDiagonalCommands.Count;

        private readonly Stack<Command> doneCardinalCommands = new();
        private readonly Stack<Command> undoneCardinalCommands = new();

        private readonly Stack<Command> doneDiagonalCommands = new();
        private readonly Stack<Command> undoneDiagonalCommands = new();

        private Stack<Command> Done(Block.Type type) =>
            type switch
            {
                Block.Type.Cardinal => doneCardinalCommands,
                Block.Type.Diagonal => doneDiagonalCommands,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        
        private Stack<Command> Undone(Block.Type type) =>
            type switch
            {
                Block.Type.Cardinal => undoneCardinalCommands,
                Block.Type.Diagonal => undoneDiagonalCommands,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

        public void Add(Command command)
        {
            Done(command.type).Push(command);
            Undone(command.type).Clear();
        }

        public bool HasRedo(Block.Type type, out Command command)
        {
            var commands = Undone(type);
            commands.TryPeek(out command);
            return commands.Any();
        }

        public bool HasUndo(Block.Type type, out Command command)
        {
            var commands = Done(type);
            commands.TryPeek(out command);
            return commands.Any();
        }

        public void Redo(Block.Type type)
        {
            if (!HasRedo(type, out var command)) return;
            Undone(type).Pop();
            Done(type).Push(command);
        }

        public void Undo(Block.Type type)
        {
            if (!HasUndo(type, out var command)) return;
            Undone(type).Push(command);
            Done(type).Pop();
        }
    }
}