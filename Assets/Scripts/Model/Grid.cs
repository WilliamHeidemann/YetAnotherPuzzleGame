using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool IsMoveValid(Move move)
        {
            if (IsAvailable(move.previous)) return false; // There must be a block where we move from
            if (!IsAvailable(move.next)) return false; // There should be space where we move to
            return true;
        }

        public void Move(Move move)
        {
            var isValid = IsMoveValid(move);
            if (!isValid) throw new Exception("Command is invalid");
            blocks.Remove(GetBlock(move.previous));
            blocks.Add(GetBlock(move.next));
        }

        private Block GetBlock(Location location)
        {
            if (IsAvailable(location)) throw new Exception($"No block at {location}");
            return blocks.First(b => b.location == location);
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