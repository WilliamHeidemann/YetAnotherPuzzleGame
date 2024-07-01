using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Model;
using ScriptableObjects;
using UnityEngine;
using UnityUtils;
using UtilityToolkit.Runtime;
using Type = Model.Type;

namespace Systems
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private MovableBlock cardinalPrefab;
        [SerializeField] private MovableBlock diagonalPrefab;
        [SerializeField] private GhostBlock ghostPrefab;
        [SerializeField] private GameObject groundPrefab;

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

        public async Task SpawnLevel(Level level)
        {
            await ClearLevel();
            InstantiateGroundBlocks(level);
            level.startingConfiguration.ForEach(InstantiateBlock);
            await Animator.BlocksIn(AllBlocks());
        }

        public async Task ResetLevel(Level level)
        {
            await Animator.ResetLevel(level.startingConfiguration, movableBlocks);
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
            ghostBlocks.Clear();
            groundBlocks.Clear();
        }

        private void InstantiateGroundBlocks(Level level)
        {
            foreach (var location in level.groundBlocks)
            {
                var position = location.asVector3.With(y: -1);
                var groundBlock = Instantiate(groundPrefab, position, Quaternion.identity);
                groundBlocks.Add(groundBlock);
                var targetOption = level.targetConfiguration.FirstOption(block => block.location == location);
                if (targetOption.IsSome(out var target))
                {
                    groundBlock.GetComponent<MeshRenderer>().material = target.material;
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
                if (!isMoveValidPredicate(neighbor)) continue;
                var ghost = Instantiate(ghostPrefab, middle.asVector3, Quaternion.identity);
                ghost.location = neighbor;
                ghost.GetComponent<MeshRenderer>().material = hover.material;
                ghostBlocks.Add(ghost);
            }
        }

        public void HideGhostBlocks()
        {
            foreach (var ghostBlock in ghostBlocks) Destroy(ghostBlock.gameObject);
            ghostBlocks.Clear();
        }

        // public void HighLightMovableBlocks(Func<Location, bool> isAvailable)
        // {
        //     foreach (var block in movableBlocks)
        //     {
        //         block.GetComponent<Outline>().enabled = block.model.neighbors.Any(isAvailable);
        //     }
        // }
    }
}