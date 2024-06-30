using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace GameState
{
    public class Grid
    {
        private readonly ICollection<Block> blocks;
        private readonly List<Location> ground;

        public Grid(IEnumerable<Block> startingBlocks, List<Location> groundBlocks)
        {
            blocks = new List<Block>();
            ground = groundBlocks;
            startingBlocks.ForEach(AddBlock);
        }

        public void AddBlock(Block block)
        {
            if (ContainsBlock(block)) return;
            if (!HasGroundAt(block.location)) return;
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
            if (!IsMoveValid(move))
            {
                Debug.LogWarning($"Move is invalid: {move}");
                return;
            }

            var blockOption = GetBlock(move.previous);
            if (!blockOption.IsSome(out var block))
                return;
            blocks.Remove(block);
            block.location = move.next;
            blocks.Add(block);
        }

        private Option<Block> GetBlock(Location location)
        {
            if (IsAvailable(location))
                throw new Exception($"No block at {location}");
            return blocks.FirstOption(b => b.location == location);
        }

        public bool ContainsBlock(Block block) => blocks.Any(b => b.location == block.location);

        public bool IsAvailable(Location location) =>
            HasGroundAt(location) && !HasBlockAt(location);

        private bool HasGroundAt(Location location) =>
            ground.Contains(location);

        private bool HasBlockAt(Location location) =>
            blocks.Any(b => b.location == location);

        public IEnumerable<Block> GetBlocks() => blocks;

        public void PrintOccupiedBlocks()
        {
            blocks.ForEach(b => Debug.Log(b.location));
        }
    }
}