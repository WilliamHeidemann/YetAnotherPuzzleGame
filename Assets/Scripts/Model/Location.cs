﻿using System;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Model
{
    [Serializable]
    public struct Location : IEquatable<Location>
    {
        public int x;
        public int y;

        public Location(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Location With(int addX = 0, int addY = 0)
        {
            return new Location(x + addX, y + addY);
        }

        public static bool operator ==(Location a, Location b) => a.x == b.x && a.y == b.y;
        public static bool operator !=(Location a, Location b) => !(a == b);

        public override string ToString() => $"(x: {x}, y: {y})";
        
        public Vector3 asVector3 => new(x, 0, y);
        public bool Equals(Location other) => x == other.x && y == other.y;
        public override bool Equals(object obj) => obj is Location other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(x, y);
    }

    public static class Vector3Extensions
    {
        public static Location AsLocation(this Vector3 vector3)
        {
            return new Location((int)vector3.x, (int)vector3.z);
        }
    }
}