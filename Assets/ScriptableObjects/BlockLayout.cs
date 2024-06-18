using System.Collections.Generic;
using Model;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Block Layout", menuName = "Block Layout", order = 0)]
    public class BlockLayout : ScriptableObject
    {
        public List<Block> list;
        public int width;
        public int maxMoves;
    }
}