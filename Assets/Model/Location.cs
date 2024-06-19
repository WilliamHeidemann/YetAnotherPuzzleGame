﻿using System;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace Model
{
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