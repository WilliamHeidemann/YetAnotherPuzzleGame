using System.Collections.Generic;
using System.Linq;
using GameState;
using Model;
using UnityEditor;
using NUnit.Framework;
using ScriptableObjects;
using UnityEngine;

namespace Tests
{
    public class LevelLayoutTests
    {

        [Test]
        public void BlockLayout_HasMatchingStartingAndTargetConfig()
        {

            var blockLayouts = 
                AssetDatabase.FindAssets("t:BlockLayout", new[] { "Assets/Levels/" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<Level>);

            foreach (var blockLayout in blockLayouts)
            {
                var startingCardinals = 
                    blockLayout.startingConfiguration.Where(block => block.type == Type.Cardinal);
                var startingDiagonals = 
                    blockLayout.startingConfiguration.Where(block => block.type == Type.Diagonal);
                var targetCardinals = 
                    blockLayout.targetConfiguration.Where(block => block.type == Type.Cardinal);
                var targetDiagonals = 
                    blockLayout.targetConfiguration.Where(block => block.type == Type.Diagonal);
                
                Assert.AreEqual(startingCardinals.Count(), targetCardinals.Count(), $"Cardinals of {blockLayout.name}");
                Assert.AreEqual(startingDiagonals.Count(), targetDiagonals.Count(), $"Diagonals of {blockLayout.name}");
            }
        }
    }
}