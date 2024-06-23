using System.Collections.Generic;
using System.Linq;
using Model;
using ScriptableObjects;
using UnityEngine;

namespace Systems
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private Level[] levels;
        private Level current;
        public bool isLevelComplete { get; private set; }
        public void CheckCompletion(IEnumerable<Block> board)
        {
            isLevelComplete = board.SequenceEqual(current.targetConfiguration);
        }
    }
}