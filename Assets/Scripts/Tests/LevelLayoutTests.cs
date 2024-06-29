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
        private IEnumerable<Level> levels;

        [SetUp]
        public void Setup()
        {
            levels = Resources.LoadAll<Level>("Levels");
        }
        
        [Test]
        public void BlockLayout_HasMatchingStartingAndTargetConfig()
        {
            foreach (var level in levels)
            {
                var startingCardinals = 
                    level.startingConfiguration.Where(block => block.type == Type.Cardinal);
                var startingDiagonals = 
                    level.startingConfiguration.Where(block => block.type == Type.Diagonal);
                var targetCardinals = 
                    level.targetConfiguration.Where(block => block.type == Type.Cardinal);
                var targetDiagonals = 
                    level.targetConfiguration.Where(block => block.type == Type.Diagonal);
                
                Debug.Log($"{level.name} has {startingCardinals.Count()} starting bobs and {targetCardinals.Count()} target bobs. ");
                
                Assert.AreEqual(startingCardinals.Count(), targetCardinals.Count(), $"Cardinals of {level.name}");
                Assert.AreEqual(startingDiagonals.Count(), targetDiagonals.Count(), $"Diagonals of {level.name}");
            }
        }

        [Test]
        public void BlockLayout_AllBlocksHaveDifferentPositions()
        {
            foreach (var level in levels)
            {
                var allStartingBlocksHaveDifferentPosition = 
                    level.startingConfiguration.Count ==
                    level.startingConfiguration.Distinct().Count();
                
                var allTargetBlocksHaveDifferentPosition = 
                    level.targetConfiguration.Count ==
                    level.targetConfiguration.Select(b => b.location).Distinct().Count();
                
                
                Assert.True(allStartingBlocksHaveDifferentPosition);
                Assert.True(allTargetBlocksHaveDifferentPosition);
            }
        }
    }
}