using System;

namespace Model
{
    public readonly struct Move
    {
        public readonly Location previous;
        public readonly Location next;
        public readonly Type type;
        public readonly bool isUndo;

        public Move(Location previous, Location next, Type type, bool isUndo = false)
        {
            this.previous = previous;
            this.next = next;
            this.type = type;
            this.isUndo = isUndo;
        }

        public override string ToString()
        {
            return $"Move: {previous} -> {next} ({type})";
        }

        public Move reversed => new(next, previous, type, true);
        public bool Equals(Move other) => previous.Equals(other.previous) && next.Equals(other.next) &&
                                          type == other.type && isUndo == other.isUndo;
        public override bool Equals(object obj) => obj is Move other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(previous, next, (int)type, isUndo);
        public static bool operator ==(Move left, Move right) => left.Equals(right);
        public static bool operator !=(Move left, Move right) => !left.Equals(right);
    }
}