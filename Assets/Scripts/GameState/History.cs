using System.Collections.Generic;
using System.Linq;
using Components;
using Model;
using UnityEngine;
using UtilityToolkit.Runtime;

namespace GameState
{
    public class History
    {
        private readonly Dictionary<MovableBlock, Stack<Location>> previousLocations = new();

        public void Add(MovableBlock block, Location previousLocation)
        {
            Debug.Log("Adding");
            if (!previousLocations.TryGetValue(block, out var stack))
            {
                stack = new Stack<Location>();
                previousLocations.Add(block, stack);
            }
            
            stack.Push(previousLocation);
        }

        public Option<Location> GetPreviousLocation(MovableBlock block)
        {
            if (!previousLocations.TryGetValue(block, out var stack))
                return Option<Location>.None;

            if (!stack.TryPeek(out var location))
                return Option<Location>.None;
            
            return Option<Location>.Some(location);
        }
        
        public void Undo(MovableBlock block)
        {
            if (!previousLocations.TryGetValue(block, out var stack))
                return;

            Debug.Log(stack.Count);
            stack.Pop();
        }
    }
}