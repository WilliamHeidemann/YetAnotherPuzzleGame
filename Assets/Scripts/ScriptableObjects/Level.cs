using System.Collections.Generic;
using Model;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Level", menuName = "Level", order = 0)]
    public class Level : ScriptableObject
    {
        public List<Block> startingConfiguration;
        public List<Block> targetConfiguration;
        public List<Location> groundBlocks;
        public int maxMoves;
    }
}