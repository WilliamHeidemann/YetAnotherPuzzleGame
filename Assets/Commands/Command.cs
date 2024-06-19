using Model;
using UnityEngine;

namespace Commands
{
    public readonly struct Command
    {
        public readonly Block previous;
        public readonly Block next;
        public readonly Block.Type type;

        public Command(Block previous, Block next, Block.Type type)
        {
            this.previous = previous;
            this.next = next;
            this.type = type;
        }
    }
}