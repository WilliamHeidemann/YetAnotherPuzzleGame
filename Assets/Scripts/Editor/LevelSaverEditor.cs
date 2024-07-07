﻿using System;
using System.Linq;
using LevelEditor;
using Model;
using ScriptableObjects;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Type = Model.Type;

namespace Editor
{
    [CustomEditor(typeof(LevelSaver))]
    public class LevelSaverEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Save To Assets"))
            {
                CreateLevelAsset();
            }
            
            if (GUILayout.Button("Select Ground"))
            {
                SelectGameObjects<GroundEditor>();
            }
            
            if (GUILayout.Button("Select Movables"))
            {
                SelectGameObjects<MovableEditor>();
            }
        }

        private void CreateLevelAsset()
        {
            var levelSaver = (LevelSaver)target;

            var ground =
                FindObjectsByType<GroundEditor>(FindObjectsSortMode.None)
                    .Where(b => b.isActive);

            var groundLocations =
                ground.Select(b => b.asLocation);
            
            var targetConfig =
                ground
                    .Where(b => b.type != BlockType.Ground)
                    .Select(b =>
                    {
                        var type = b.type switch
                        {
                            BlockType.Cardinal => Type.Cardinal,
                            BlockType.Diagonal => Type.Diagonal,
                            BlockType.Frog => Type.Frog,
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        return new Block(b.asLocation, type);
                    });
            
            var startConfig =
                FindObjectsByType<MovableEditor>(FindObjectsSortMode.None)
                    .Where(b => b.isActive)
                    .Select(b => b.asBlock);
            
            var maxMoves = levelSaver.maxMoves;


            var level = CreateInstance<Level>();
            level.groundBlocks = groundLocations.ToList();
            level.startingConfiguration = startConfig.ToList();
            level.targetConfiguration = targetConfig.ToList();
            level.maxMoves = maxMoves;

            var path = "Assets/Resources/Levels/New Level.asset";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            AssetDatabase.CreateAsset(level, path);
            AssetDatabase.SaveAssets();
            Selection.activeObject = level;
        }

        private static void SelectGameObjects<T>() where T : Component
        {
            var selectedObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                .Where(obj => obj.GetComponent<T>() != null)
                .Select(g => g as Object)
                .ToArray();

            Selection.objects = selectedObjects;
        }
    }
}