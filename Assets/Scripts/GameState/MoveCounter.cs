using System;
using TMPro;
using UtilityToolkit.Runtime;

namespace GameState
{
    public class MoveCounter
    {
        private readonly Observable<int> moveCount;
        private readonly int max;
        public bool hasMovesLeft => moveCount.Value < max;
        
        public void IncrementCount()
        {
            if (moveCount.Value == max) throw new Exception("Trying to increment move count above max.");
            moveCount.Value++;
        }

        public void DecrementCount()
        {
            if (moveCount.Value == 0) throw new Exception("Trying to decrement move count below zero.");
            moveCount.Value--;
        }

        public MoveCounter(int maxCount)
        {
            max = maxCount;
            moveCount = new Observable<int>();
            MoveCounterComponent.Instance.SetCircles(maxCount);
            moveCount.OnValueChanged += MoveCounterComponent.Instance.FillCircles;
        }
    }
}