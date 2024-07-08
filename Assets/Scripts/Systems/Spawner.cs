﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Model;
using ScriptableObjects;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;
using Animator = Animation.Animator;
using Type = Model.Type;

namespace Systems
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private MovableBlock cardinalPrefab;
        [SerializeField] private MovableBlock diagonalPrefab;
        [SerializeField] private MovableBlock frogPrefab;
        [SerializeField] private GroundBlock groundPrefab;
        [SerializeField] private GameObject highlightPrefab;

        private readonly List<MovableBlock> movableBlocks = new();
        private readonly List<GroundBlock> groundBlocks = new();
        private readonly List<GameObject> highlights = new();

        private IEnumerable<GameObject> AllBlocks()
        {
            var movables = movableBlocks.Select(b => b.gameObject);
            var lights = highlights.Select(b => b.gameObject);
            var ground = groundBlocks.Select(b => b.gameObject);
            return movables.Concat(lights).Concat(ground);
        }

        public async Task SpawnLevel(Level level)
        {
            await ClearLevel();
            InstantiateGroundBlocks(level.groundBlocks);
            ColorGroundBlocks(level);
            level.startingConfiguration.ForEach(InstantiateBlock);
            await Animator.BlocksIn(AllBlocks());
        }

        public async Task ResetLevel(Level level)
        {
            await Animator.ResetLevel(level.startingConfiguration, movableBlocks);
        }

        public async void RealignGroundBlocks(List<GameObject> availableBlocks, Level level)
        {
            // if (availableBlocks.Count > level.groundBlocks)
            // {
            //     
            // }
            //
            //
            // while (gameObjectEnumerator.MoveNext())
            // {
            //     var groundLocation = levelEnumerator.Current;
            //     var groundBlock = gameObjectGround.AddComponent<GroundBlock>();
            //     groundBlock.location = groundLocation;
            //     groundBlocks.Add(groundBlock);
            //     groundBlock.transform.position = groundLocation.asVector3.With(y: -1);
            //     if (!levelEnumerator.MoveNext())
            //     {
            //         // spawn the rest and quit
            //     }
            // }
            
            

            
            // foreach available
                // add to ground blocks
                // set correct position
                // Tween it individually
                
            // if there are any left overs, animate them out
            // if there are not enough, spawn the rest needed
                
            // Change to BlocksIn dont move these blocks also
            
            ColorGroundBlocks(level);
        }

        public Option<MovableBlock> GetMovableBlock(Location location) =>
            movableBlocks.FirstOption(b => b.model.location == location);

        private async Task ClearLevel()
        {
            var allBlocks = AllBlocks();
            if (!allBlocks.Any()) return;
            await Animator.BlocksOut(AllBlocks());
            AllBlocks().ForEach(b => Destroy(b.gameObject, 2f));
            movableBlocks.Clear();
            groundBlocks.Clear();
            highlights.Clear();
        }

        private void InstantiateGroundBlocks(List<Location> groundBlocksToSpawn)
        {
            foreach (var location in groundBlocksToSpawn)
            {
                var position = location.asVector3.With(y: -1);
                var groundBlock = Instantiate(groundPrefab, position, Quaternion.identity);
                groundBlocks.Add(groundBlock);
                groundBlock.location = location;
            }
        }

        private void ColorGroundBlocks(Level level)
        {
            foreach (var block in level.targetConfiguration)
            {
                if (groundBlocks.FirstOption(g => g.location == block.location).IsSome(out var ground))
                {
                    ground.GetComponent<MeshRenderer>().material = block.material;
                }
            }
        }

        private void InstantiateBlock(Block block)
        {
            var prefab = block.type switch
            {
                Type.Cardinal => cardinalPrefab,
                Type.Diagonal => diagonalPrefab,
                Type.Frog => frogPrefab,
                _ => throw new ArgumentOutOfRangeException()
            };

            var position = block.location.asVector3;
            var movableBlock = Instantiate(prefab, position, Quaternion.identity);
            movableBlock.model = block;
            movableBlocks.Add(movableBlock);
        }

        public void ShowHighlights(IEnumerable<Location> locationsToHighlight)
        {
            foreach (var location in locationsToHighlight)
            {
                var highlight = Instantiate(highlightPrefab, location.asVector3, highlightPrefab.transform.rotation);
                highlights.Add(highlight);
            }
        }

        public void HideHighlights()
        {
            foreach (var highlight in highlights)
                Destroy(highlight.gameObject);
            highlights.Clear();
        }
    }
}