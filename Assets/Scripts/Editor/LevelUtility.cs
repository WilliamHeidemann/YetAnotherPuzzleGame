using System.Collections.Generic;
using Model;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Level)), CanEditMultipleObjects]
    public class LevelUtility : UnityEditor.Editor
    {
        private bool confirmCopy;
        private bool confirm3X3GroundBlocks;

        public override void OnInspectorGUI()
        {
            var level = (Level)target;

            if (level.image != null)
            {
                GUILayout.Label(level.image, GUILayout.Width(256), GUILayout.Height(256));
            }

            base.OnInspectorGUI();

            confirmCopy = EditorGUILayout.Toggle("Confirm Copy", confirmCopy);
            EditorGUI.BeginDisabledGroup(!confirmCopy);
            if (GUILayout.Button("Duplicate starting config to target config"))
            {
                DuplicateStartToTarget(level);
            }
            EditorGUI.EndDisabledGroup();
            
            confirm3X3GroundBlocks = EditorGUILayout.Toggle("Confirm 3x3 Ground", confirm3X3GroundBlocks);
            EditorGUI.BeginDisabledGroup(!confirm3X3GroundBlocks);
            if (GUILayout.Button("Set ground to 3x3 grid"))
            {
                Set3X3GroundBlocks(level);
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DuplicateStartToTarget(Level level)
        {
            level.targetConfiguration = new List<Block>(level.startingConfiguration);
            confirmCopy = false;
            EditorUtility.SetDirty(level);
        }

        private void Set3X3GroundBlocks(Level level)
        {
            var groundBlocks = new List<Location>
            {
                new(0, 0),
                new(1, 0),
                new(2, 0),
                new(0, 1),
                new(1, 1),
                new(2, 1),
                new(0, 2),
                new(1, 2),
                new(2, 2),
            };
            confirm3X3GroundBlocks = false;
            level.groundBlocks = groundBlocks;
            EditorUtility.SetDirty(level);
        }
    }
}