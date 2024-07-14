using System.Linq;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using UtilityToolkit.Editor;

namespace LevelEditor
{
    public class LevelSaver : MonoBehaviour
    {
        public int maxMoves;
        public bool hasInfiniteMoves;

        public Level levelToLoad;
    }
}