using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UnityUtils;

namespace GameState
{
    public class Grid
    {
        private readonly ICollection<Block> blocks;
        private readonly int width;
        private readonly int height;

        public Grid(int width, int height, IEnumerable<Block> startingBlocks)
        {
            blocks = new List<Block>();
            this.width = width;
            this.height = height;
            startingBlocks.ForEach(AddBlock);
        }

        public void AddBlock(Block block)
        {
            if (ContainsBlock(block)) return;
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
            if (!isValid) 
                throw new Exception("Command is invalid");
            var block = GetBlock(move.previous);
            blocks.Remove(block);
            block.location = move.next;
            blocks.Add(block);
        }

        private Block GetBlock(Location location)
        {
            if (IsAvailable(location)) 
                throw new Exception($"No block at {location}");
            return blocks.First(b => b.location == location);
        }
        
        public bool ContainsBlock(Block block) => blocks.Any(b => b.location == block.location);

        public bool IsAvailable(Location location) =>
            IsWithinBounds(location) && !HasBlockAt(location);

        private bool IsWithinBounds(Location location) =>
            location.x >= 0 && location.y >= 0 &&
            location.x < width && location.y < height;

        private bool HasBlockAt(Location location) =>
            blocks.Any(b => b.location == location);

        public IEnumerable<Block> GetBlocks() => blocks;

        public void PrintOccupiedBlocks()
        {
            blocks.ForEach(b => Debug.Log(b.location));
        }
    }
}