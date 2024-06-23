using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Model;
using ScriptableObjects;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;
using Grid = UnityEngine.Grid;
using Type = Model.Type;

namespace Systems
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private MovableBlock cardinalPrefab;
        [SerializeField] private MovableBlock diagonalPrefab;
        [SerializeField] private GhostBlock ghostPrefab;
        [SerializeField] private GameObject groundPrefab;
        [SerializeField] private Material blue;
        [SerializeField] private Material red;

        private readonly List<MovableBlock> movableBlocks = new();
        private readonly List<GhostBlock> ghostBlocks = new();
        private readonly List<GameObject> groundBlocks = new();

        private IEnumerable<GameObject> AllBlocks()
        {
            var movables = movableBlocks.Select(b => b.gameObject);
            var ghosts = ghostBlocks.Select(b => b.gameObject);
            var ground = groundBlocks.Select(b => b.gameObject);
            return movables.Concat(ghosts).Concat(ground);
        }

        public void SpawnLevel(Level level)
        {
            ClearLevel();
            InstantiateGroundBlocks(level);
            level.startingConfiguration.ForEach(InstantiateBlock);
            LevelAnimator.BlocksIn(AllBlocks());
        }

        private void ClearLevel()
        {
            LevelAnimator.BlocksOut(AllBlocks());
            AllBlocks().ForEach(b => Destroy(b.gameObject, 2f));
            movableBlocks.Clear();
            ghostBlocks.Clear();
            groundBlocks.Clear();
        }


        private void InstantiateGroundBlocks(Level level)
        {
            for (int i = 0; i < level.height; i++)
            {
                for (int j = 0; j < level.width; j++)
                {
                    var position = new Vector3(j, -1, i);
                    var groundBlock = Instantiate(groundPrefab, position, Quaternion.identity);
                    groundBlocks.Add(groundBlock);
                    var location = new Location(j, i);
                    var targetOption = level.targetConfiguration.FirstOption(block => block.location == location);
                    if (targetOption.IsSome(out var target))
                    {
                        groundBlock.GetComponent<MeshRenderer>().material = target.type switch
                        {
                            Type.Cardinal => blue,
                            Type.Diagonal => red,
                            _ => throw new ArgumentOutOfRangeException()
                        };
                    }
                }
            }
        }

        private void InstantiateBlock(Block block)
        {
            var prefab = block.type switch
            {
                Type.Cardinal => cardinalPrefab,
                Type.Diagonal => diagonalPrefab,
                _ => throw new ArgumentOutOfRangeException()
            };

            var position = block.location.asVector3;
            var movableBlock = Instantiate(prefab, position, Quaternion.identity);
            movableBlock.model = block;
            movableBlocks.Add(movableBlock);
        }

        public void ShowGhostBlocks(Block hover, Func<Location, bool> isMoveValidPredicate)
        {
            HideGhostBlocks();

            var middle = hover.location;
            var neighbors = hover.neighbors;

            foreach (var neighbor in neighbors)
            {
                if (isMoveValidPredicate(neighbor)) continue;
                var ghost = Instantiate(ghostPrefab, middle.asVector3, Quaternion.identity);
                ghost.location = neighbor;
                ghost.GetComponent<MeshRenderer>().material = hover.type switch
                {
                    Type.Cardinal => blue,
                    Type.Diagonal => red,
                    _ => throw new ArgumentOutOfRangeException()
                };
                ghostBlocks.Add(ghost);
            }
        }

        public void HideGhostBlocks()
        {
            foreach (var ghostBlock in ghostBlocks) Destroy(ghostBlock.gameObject);
            ghostBlocks.Clear();
        }
    }
}