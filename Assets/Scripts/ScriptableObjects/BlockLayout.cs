using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Block Layout", menuName = "Block Layout", order = 0)]
    public class BlockLayout : ScriptableObject
    {
        public List<Block> startingConfiguration;
        public List<Block> targetConfiguration;
        public int width;
        public int height;
        public int maxMoves;
    }
}