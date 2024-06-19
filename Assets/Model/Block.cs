using System;

namespace Model
{
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

        public override string ToString()
        {
            return location.ToString();
        }
    }
}