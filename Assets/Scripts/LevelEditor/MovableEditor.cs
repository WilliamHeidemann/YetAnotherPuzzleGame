using Model;
using UnityEngine;

namespace LevelEditor
{
    public class MovableEditor : MonoBehaviour
    {
        public Type type;
        public Block asBlock => new Block(new Location((int)transform.position.x, (int)transform.position.y), type);
    }
}
