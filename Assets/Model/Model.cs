using System;
using System.Collections.Generic;
using System.Linq;
using Commands;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Model
{
    public class Grid
    {
        private readonly ICollection<Block> blocks;
        private readonly int width;
        public Grid(int width)
        {
            blocks = new List<Block>();
            this.width = width;
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
            location.x < width && location.y < width &&
            blocks.All(b => b.location != location);
    }

    [Serializable]
    public struct Block
    {
        public Location location;
        public Type type;

        public Block(Location location, Type type)
        {
            this.location = location;
            this.type = type;
        }
        
        public enum Type
        {
            Cardinal,
            Diagonal
        }

        public Location[] neighbors => type switch
        {
            Type.Cardinal => cardinals,
            Type.Diagonal => diagonals,
            _ => throw new ArgumentOutOfRangeException()
        };

        private Location[] cardinals => new[]
        {
            new Location(location.x + 1, location.y),
            new Location(location.x - 1, location.y),
            new Location(location.x, location.y + 1),
            new Location(location.x, location.y - 1),
        };
        
        private Location[] diagonals => new[]
        {
            new Location(location.x + 1, location.y + 1),
            new Location(location.x - 1, location.y - 1),
            new Location(location.x - 1, location.y + 1),
            new Location(location.x + 1, location.y - 1),
        };
    }
    
    [Serializable]
    public struct Location
    {
        public int x;
        public int y;

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Location a, Location b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Location a, Location b) => !(a == b);

        public override string ToString() => $"x: {x}, y: {y}";
        
        public Vector3 asVector3 => new(x, 0, y);
    }
}