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

        public Move reversed => new(next, previous, type, !isUndo);
    }
}