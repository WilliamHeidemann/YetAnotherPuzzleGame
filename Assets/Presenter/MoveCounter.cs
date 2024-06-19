using TMPro;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;

namespace Presenter
{
    public class MoveCounter : Singleton<MoveCounter>
    {
        [SerializeField] private TextMeshProUGUI text;
        private int maxMoves;
        
        public void Subscribe(Level.MoveData moveData)
        {
            maxMoves = moveData.max;
            moveData.used.OnValueChanged += UpdateText;
            UpdateText(0);
        }

        private void UpdateText(int used)
        {
            text.text = $"{used} / {maxMoves}";
        }
    }
}
