using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Location> GetAvailableNeighbors(Func<Location, bool> hasBlockAt,
            Func<Location, bool> hasGroundAt)
        {
            return type switch
            {
                Type.Cardinal => cardinals.Where(l => !hasBlockAt(l) && hasGroundAt(l)),
                Type.Diagonal => diagonals.Where(l => !hasBlockAt(l) && hasGroundAt(l)),
                Type.Frog => FrogNeighbors(hasBlockAt, hasGroundAt),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public Material material => Resources.Load<Material>($"Materials/{type.ToString()}");

        private Location[] cardinals => new[]
        {
            location.With(addX: 1),
            location.With(addX: -1),
            location.With(addY: 1),
            location.With(addY: -1),
        };

        private Location[] diagonals => new[]
        {
            location.With(1, 1),
            location.With(-1, -1),
            location.With(-1, 1),
            location.With(1, -1),
        };

        public IEnumerable<Location> FrogNeighbors(Func<Location, bool> hasBlockAt, Func<Location, bool> hasGroundAt)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (!hasBlockAt(location.With(j, i))) continue;
                    var neighbor = location.With(j * 2, i * 2);
                    if (hasBlockAt(neighbor) || !hasGroundAt(neighbor)) continue;
                    yield return neighbor;
                }
            }
        }

        public override string ToString() => location.ToString();
        public static bool operator ==(Block left, Block right) => left.Equals(right);
        public static bool operator !=(Block left, Block right) => !left.Equals(right);
        public bool Equals(Block other) => location.Equals(other.location) && type == other.type;
        public override bool Equals(object obj) => obj is Block other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(location, (int)type);
    }
}