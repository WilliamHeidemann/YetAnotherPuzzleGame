using System.Collections.Generic;
using System.Linq;
using Commands;
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
        public void TestBlockLayout()
        {

            var blockLayouts = 
                AssetDatabase.FindAssets("t:BlockLayout", new[] { "Assets/Levels/" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<BlockLayout>);

            foreach (var blockLayout in blockLayouts)
            {
                var startingCardinals = 
                    blockLayout.startingConfiguration.Where(block => block.type == Block.Type.Cardinal);
                var startingDiagonals = 
                    blockLayout.startingConfiguration.Where(block => block.type == Block.Type.Diagonal);
                var targetCardinals = 
                    blockLayout.targetConfiguration.Where(block => block.type == Block.Type.Cardinal);
                var targetDiagonals = 
                    blockLayout.targetConfiguration.Where(block => block.type == Block.Type.Diagonal);
                
                Assert.AreEqual(startingCardinals.Count(), targetCardinals.Count(), $"Cardinals of {blockLayout.name}");
                Assert.AreEqual(startingDiagonals.Count(), targetDiagonals.Count(), $"Diagonals of {blockLayout.name}");
            }
        }

        [Test]
        public void TestHistory()
        {
            var block1 = new Block(new Location(0,0), Block.Type.Cardinal);
            var block2 = new Block(new Location(1,0), Block.Type.Cardinal);
            var command = new Command(block1, block2, Block.Type.Cardinal);
            var history = new History();
            
            history.Add(command);
            Assert.AreEqual(history.count, 1);
            Assert.AreEqual(history.doneCardinalCount, 1);
            Assert.AreEqual(history.undoneCardinalCount, 0);
            
            history.Undo(Block.Type.Cardinal);
            Assert.AreEqual(history.count, 0);
            Assert.AreEqual(history.doneCardinalCount, 0);
            Assert.AreEqual(history.undoneCardinalCount, 1);

            history.Redo(Block.Type.Cardinal);
            Assert.AreEqual(history.count, 1);
            Assert.AreEqual(history.doneCardinalCount, 1);
            Assert.AreEqual(history.undoneCardinalCount, 0);
            
            history.Add(command);
            Assert.AreEqual(history.count, 2);
            Assert.AreEqual(history.doneCardinalCount, 2);
            Assert.AreEqual(history.undoneCardinalCount, 0);

            history.Undo(Block.Type.Cardinal);
            Assert.AreEqual(history.count, 1);
            Assert.AreEqual(history.doneCardinalCount, 1);
            Assert.AreEqual(history.undoneCardinalCount, 1);
            
            history.Undo(Block.Type.Cardinal);
            Assert.AreEqual(history.count, 0);
            Assert.AreEqual(history.doneCardinalCount, 0);
            Assert.AreEqual(history.undoneCardinalCount, 2);
        }
    }
}