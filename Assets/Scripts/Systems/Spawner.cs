using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components;
using MainMenu;
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
            SpawnGroundBlocks(level);
            AnimateGroundBlocks(level);
            ColorGroundBlocks(level);
            
            SpawnMovableBlocks(level);
            AnimateMovableBlocks(level);
            highlights.Clear();
        }

        private void AnimateMovableBlocks(Level level)
        {
            
        }

        private void AnimateGroundBlocks(Level level)
        {
            For.Range(groundBlocks.Count,
                i => Animator.Move(groundBlocks[i].gameObject, level.groundBlocks[i].asVector3.With(y: -1), Type.Cardinal));
        }


        private void SpawnGroundBlocks(Level level)
        {
            groundBlocks.AddRange(GetLevelButtonBlocks());

            if (groundBlocks.Count > level.groundBlocks.Count)
            {
                var amountDiscard = groundBlocks.Count - level.groundBlocks.Count;
                var blocksToDiscard = groundBlocks.Take(amountDiscard);
                foreach (var groundBlock in blocksToDiscard)
                {
                    groundBlocks.Remove(groundBlock);
                }
                // animate blocksToDiscard away
            }
            else
            {
                var amountAdd = level.groundBlocks.Count - groundBlocks.Count;
                For.Range(amountAdd, i => InstantiateGroundBlock(level.groundBlocks[i]));
            }

            // now groundBlocks.Count == level.groundBlocks.Count

            // animate all ground blocks to their respective locations (some of them are already there if they were just spawned)
            // do so the total travel distance is minimized

            // give them all their correct color
        }

        private void InstantiateGroundBlock(Location location)
        {
            var position = location.asVector3.With(y: -1);
            var groundBlock = Instantiate(groundPrefab, position, Quaternion.identity);
            groundBlock.location = location;
            groundBlocks.Add(groundBlock);
        }

        private IEnumerable<GroundBlock> GetLevelButtonBlocks()
        {
            var levelButtonBlocks =
                FindObjectsByType<LevelButton>(FindObjectsSortMode.None)
                    .Select(b => b.gameObject)
                    .ToList();

            levelButtonBlocks.ForEach(b =>
            {
                Destroy(b.GetComponent<LevelButton>());
                var groundBlock = b.AddComponent<GroundBlock>();
                groundBlocks.Add(groundBlock);
            });

            return levelButtonBlocks.Select(b => b.GetComponent<GroundBlock>());
        }

        private void SpawnMovableBlocks(Level level)
        {
            For.Each<Type>(t => AdjustMovableBlocksOfType(t, level));
            // now movableBlocks.Count == level.startingConfiguration.Count
            // // animate all blocks to their respective locations
        }

        private void AdjustMovableBlocksOfType(Type type, Level level)
        {
            if (movableBlocks.Count(b => b.model.type == type) > level.startingConfiguration.Count(b => b.type == type))
            {
                var amountDiscard = movableBlocks.Count - level.startingConfiguration.Count(b => b.type == type);
                var blocksToDiscard = movableBlocks.Where(b => b.model.type == type).Take(amountDiscard);
                foreach (var block in blocksToDiscard)
                {
                    movableBlocks.Remove(block);
                }
                // animate blocksToDiscard away
            }
            else
            {
                var amountAdd = level.startingConfiguration.Count - movableBlocks.Count;
                For.Range(amountAdd, i => InstantiateMovableBlock(level.startingConfiguration[i], type));
            }
        }

        private void InstantiateMovableBlock(Block block, Type type)
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

        public async Task ResetLevel(Level level)
        {
            await Animator.ResetLevel(level.startingConfiguration, movableBlocks);
        }

        public Option<MovableBlock> GetMovableBlock(Location location) =>
            movableBlocks.FirstOption(b => b.model.location == location);

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