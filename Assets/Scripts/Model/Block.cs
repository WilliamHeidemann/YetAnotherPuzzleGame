using System;
using UnityEngine;

namespace Model
{
    [Serializable]
    public struct Block : IEquatable<Block>
    {
        public Location location;
        public Type type;

        public Block(Location location, Type type)
        {
            this.location = location;
            this.type = type;
        }

        public Block WithLocation(Location newLocation) => new(newLocation, type);

        public Location[] neighbors => type switch
        {
            Type.Cardinal => cardinals,
            Type.Diagonal => diagonals,
            _ => throw new ArgumentOutOfRangeException()
        };

        public Material material => Resources.Load<Material>($"Materials/{type.ToString()}");

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
        
        public override string ToString() => location.ToString();
        public static bool operator ==(Block left, Block right) => left.Equals(right);
        public static bool operator !=(Block left, Block right) => !left.Equals(right);
        public bool Equals(Block other) => location.Equals(other.location) && type == other.type;
        public override bool Equals(object obj) => obj is Block other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(location, (int)type);
    }
}