using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Model;
using ScriptableObjects;
using UnityEngine;
using UtilityToolkit.Runtime;
using Grid = UnityEngine.Grid;
using Type = Model.Type;

namespace Systems
{
    public class LevelSpawner : MonoBehaviour
    {
        [SerializeField] private List<BlockLayout> levels;
        
        [SerializeField] private MovableBlock cardinalPrefab;
        [SerializeField] private MovableBlock diagonalPrefab;
        [SerializeField] private GhostBlock ghostPrefab;
        [SerializeField] private GameObject groundPrefab;
        [SerializeField] private Material blue;
        [SerializeField] private Material red;

        
        private readonly List<MovableBlock> movableBlocks = new();
        private readonly List<GhostBlock> ghostBlocks = new();
        private readonly List<GameObject> groundBlocks = new();
        
        private BlockLayout currentLevel;
        private int currentLevelIndex;

        
        protected void Awake()
        {
            // base.Awake();
            ClearLevel();
            CreateLevel(levels[0]);
        }

        private IEnumerable<GameObject> AllBlocks()
        {
            var movables = movableBlocks.Select(b => b.gameObject);
            var ghosts = ghostBlocks.Select(b => b.gameObject);
            var ground = groundBlocks.Select(b => b.gameObject);
            return movables.Concat(ghosts).Concat(ground);
        }

        private void ClearLevel()
        {
            LevelAnimator.BlocksOut(AllBlocks());
            AllBlocks().ForEach(b => Destroy(b.gameObject, 2f));
            movableBlocks.Clear();
            ghostBlocks.Clear();
            groundBlocks.Clear();
            isLevelComplete = false;
        }

        private void CreateLevel(BlockLayout level)
        {
            if (level == null) throw new Exception("Block Layout Scriptable Object has not been set.");
            currentLevel = level;
            currentLevelIndex++;

            For.NestedRange(level.height, level.width, InstantiateGroundBlock);

            foreach (var block in level.startingConfiguration)
            {
                grid.AddBlock(block);
                InstantiateBlock(block);
            }

            LevelAnimator.BlocksIn(AllBlocks());
        }

        private void InstantiateGroundBlock(int i, int j)
        {
            var position = new Vector3(j, -1, i);
            var groundBlock = Instantiate(groundPrefab, position, Quaternion.identity);
            groundBlocks.Add(groundBlock);
            var location = new Location(j, i);
            if (currentLevel.targetConfiguration.Any(block => block.location == location))
            {
                var block = currentLevel.targetConfiguration.First(block => block.location == location);
                groundBlock.GetComponent<MeshRenderer>().material =
                    block.type == Type.Cardinal ? blue : red;
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
        
        public void ShowGhostBlocks(Block hover)
        {
            HideGhostBlocks();

            var middle = hover.location;
            var neighbors = hover.neighbors;

            foreach (var neighbor in neighbors)
            {
                if (!grid.IsAvailable(neighbor)) continue;
                var ghost = Instantiate(ghostPrefab, middle.asVector3, Quaternion.identity);
                ghost.model = new Block(neighbor, hover.type);
                ghost.GetComponent<MeshRenderer>().material = hover.type == Type.Cardinal ? blue : red;
                ghostBlocks.Add(ghost);
            }
        }

        public void HideGhostBlocks()
        {
            foreach (var ghostBlock in ghostBlocks) Destroy(ghostBlock.gameObject);
            ghostBlocks.Clear();
        }

        private async void NextLevel()
        {
            await Awaitable.WaitForSecondsAsync(1);
            ClearLevel();
            await Awaitable.WaitForSecondsAsync(2);
            CreateLevel(levels[currentLevelIndex]);
        }

    }
}