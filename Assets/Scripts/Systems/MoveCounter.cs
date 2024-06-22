using TMPro;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace Systems
{
    public class MoveCounter : Singleton<MoveCounter>
    {
        [SerializeField] private TextMeshProUGUI text;

        private Observable<int> moveCount;
        private int max;
        public int count => moveCount.Value;
        public void IncrementCount() => moveCount.Value++;
        public void DecrementCount() => moveCount.Value--;

        public void Initialize(int maxCount)
        {
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