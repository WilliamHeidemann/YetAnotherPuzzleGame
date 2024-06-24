namespace Model
{
    public readonly struct Move
    {
        public readonly Location previous;
        public readonly Location next;
        public readonly Type type;

        public Move(Location previous, Location next, Type type)
        {
            this.previous = previous;
            this.next = next;
            this.type = type;
        }

        public override string ToString()
        {
            return $"Move: {previous} -> {next} ({type})";
        }
    }
}