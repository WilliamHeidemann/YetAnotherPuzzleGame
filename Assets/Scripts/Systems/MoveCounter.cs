using TMPro;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace Systems
{
    public class MoveCounter
    {
        private readonly TextMeshProUGUI text;
        private readonly Observable<int> moveCount;
        private readonly int max;
        public int count => moveCount.Value;
        public void IncrementCount() => moveCount.Value++;
        public void DecrementCount() => moveCount.Value--;

        public MoveCounter(int maxCount, TextMeshProUGUI countingText)
        {
            text = countingText;
            max = maxCount;
            moveCount = new Observable<int>();
            moveCount.OnValueChanged += UpdateText;
        }

        private void UpdateText(int used)
        {
            text.text = $"{used} / {max}";
        }
    }
}