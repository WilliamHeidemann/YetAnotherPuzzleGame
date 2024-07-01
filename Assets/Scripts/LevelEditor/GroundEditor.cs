using Model;
using UnityEngine;

namespace LevelEditor
{
    public class GroundEditor : MonoBehaviour
    {
        public bool isActive;
        public Location asLocation => new((int)transform.position.x, (int)transform.position.y);
        public BlockType type;
    }

    public enum BlockType
    {
        Cardinal, 
        Diagonal, 
        Ground
    }
}