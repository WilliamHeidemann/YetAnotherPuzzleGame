using Model;
using UnityEngine;

namespace LevelEditor
{
    public class GroundEditor : MonoBehaviour
    {
        public bool isActive;
        public Location asLocation => transform.position.AsLocation();
        public BlockType type;
        
        private void OnValidate()
        {
            var materialName = isActive ? type.ToString() : "Transparent";
            GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/{materialName}");
        }
    }

    public enum BlockType
    {
        Cardinal, 
        Diagonal, 
        Ground
    }
}