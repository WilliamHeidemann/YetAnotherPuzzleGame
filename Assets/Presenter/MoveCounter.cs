using TMPro;
using UnityEngine;

namespace Presenter
{
    public class MoveCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        private int maxMoves;

        private void Start()
        {
            var moveData = Level.Instance.moveData;
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
