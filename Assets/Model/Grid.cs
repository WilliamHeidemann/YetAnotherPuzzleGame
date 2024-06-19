using System;
using System.Collections.Generic;
using System.Linq;
using Commands;
using UnityEngine;

namespace Model
{
    public class Grid
    {
        private readonly ICollection<Block> blocks;
        private readonly int width;
        private readonly int height;

        public Grid(int width, int height)
        {
            blocks = new List<Block>();
            this.width = width;
            this.height = height;
        }

        public void AddBlock(Block block)
        {
            if (Contains(block)) return;
            blocks.Add(block);
        }

        public bool IsMoveValid(Command command)
        {
            if (!Contains(command.previous)) return false; // There must be a block where we move from
            if (Contains(command.next)) return false; // There should be space where we move to
            return true;
        }

        public bool IsUndoMoveValid(Command command)
        {
            if (!Contains(command.next)) return false; // There must be a block where we move from
            if (Contains(command.previous)) return false; // There should be space where we move to
            return true;
        }

        public void Move(Command command, bool isUndo = false)
        {
            var isValid = isUndo ? IsUndoMoveValid(command) : IsMoveValid(command);
            if (!isValid) throw new Exception("Command is invalid");
            blocks.Remove(isUndo ? command.next : command.previous);
            blocks.Add(isUndo ? command.previous : command.next);
        }

        private bool Contains(Block block) => blocks.Any(b => b.location == block.location);

        public bool IsAvailable(Location location) =>
            location.x >= 0 && location.y >= 0 &&
            location.x < width && location.y < height &&
            blocks.All(b => b.location != location);

        public IEnumerable<Block> GetBlocks() => blocks;

        public void PrintOccupiedBlocks()
        {
            blocks.ForEach(b => Debug.Log(b.location));
        }
    }
}