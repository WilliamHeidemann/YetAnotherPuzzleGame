using System;
using Model;
using UnityEngine;
using Type = Model.Type;

namespace LevelEditor
{
    public class MovableEditor : MonoBehaviour
    {
        public bool isActive;
        public Type type;
        public Block asBlock => new(transform.position.AsLocation(), type);

        private void OnValidate()
        {
            var materialName = isActive ? type.ToString() : "Transparent";
            GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/{materialName}");
        }
    }
}
