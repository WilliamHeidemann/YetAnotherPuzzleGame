using System.Collections.Generic;
using Model;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Level))]
    public class LevelEditor : UnityEditor.Editor
    {
        private bool confirmCopy;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            confirmCopy = EditorGUILayout.Toggle("Confirm Copy", confirmCopy);
            var level = (Level)target;
            EditorGUI.BeginDisabledGroup(!confirmCopy);
            if (GUILayout.Button("Duplicate starting config to target config"))
            {
                level.targetConfiguration = new List<Block>(level.startingConfiguration);
                confirmCopy = false;
                EditorUtility.SetDirty(level);
            }
            EditorGUI.EndDisabledGroup();
        }
    }
}