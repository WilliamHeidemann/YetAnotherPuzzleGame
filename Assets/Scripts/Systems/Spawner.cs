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
using Random = UnityEngine.Random;
using Type = Model.Type;

namespace Systems
{
    public class Spawner : Singleton<Spawner>
    {
        [SerializeField] private MovableBlock cardinalPrefab;
        [SerializeField] private MovableBlock diagonalPrefab;
        [SerializeField] private MovableBlock frogPrefab;
        [SerializeField] private GroundBlock groundPrefab;
        [SerializeField] private GameObject highlightPrefab;

        [SerializeField] private GameObject trialWorldMenuBlocks;
        [SerializeField] private GameObject worldOneMenuBlocks;
        [SerializeField] private GameObject frogWorldMenuBlocks;

        private readonly List<MovableBlock> movableBlocks = new();
        private readonly List<GroundBlock> groundBlocks = new();
        private readonly List<GameObject> highlights = new();
        private readonly List<Location> highlightLocations = new();
        private readonly List<GameObject> menuBlocks = new();

        public MovableBlock getFirstBlock => movableBlocks.First();
        public Option<MovableBlock> GetMovableBlock(Location location) =>
            movableBlocks.FirstOption(b => b.model.location == location);

        public void ShowHighlights(List<Location> locationsToHighlight)
        {
            if (locationsToHighlight.All(highlightLocations.Contains) &&
                highlightLocations.All(locationsToHighlight.Contains))
                return;

            HideHighlights();
            foreach (var location in locationsToHighlight)
            {
                var highlight = Instantiate(highlightPrefab, location.asVector3, highlightPrefab.transform.rotation);
                highlights.Add(highlight);
                highlightLocations.Add(location);
            }
        }

        public void HideHighlights()
        {
            foreach (var highlight in highlights)
                Destroy(highlight.gameObject);
            highlights.Clear();
            highlightLocations.Clear();
        }

        public async Task ResetLevel(Level level)
        {
            await Animator.ResetLevel(level.startingConfiguration, movableBlocks);
        }

        public void InitLevelButtons()
        {
            menuBlocks.Clear();
            var parent = LevelManager.Instance.world switch
            {
                LevelManager.World.Trial => trialWorldMenuBlocks,
                LevelManager.World.One => worldOneMenuBlocks,
                LevelManager.World.Frog => frogWorldMenuBlocks,
                _ => throw new ArgumentOutOfRangeException()
            };
            parent.transform.ForEveryChild(t =>
            {
                var block = Instantiate(t.gameObject);
                menuBlocks.Add(block);
                block.SetActive(true);
            });

            // load status of all levels and set their status accordingly
            var levelButtons = menuBlocks
                .Select(g => g.GetComponent<LevelButton>())
                .Where(b => b != null);
                
            levelButtons.ForEach(b =>
            {
                b.status = LevelManager.Instance.GetStatus(b.index);
                var material = Resources.Load<Material>($"Materials/{b.status}");
                b.GetComponent<MeshRenderer>().material = material;
                b.UpdateImage();
            });
            
            
            // set availability visual accordingly
            // Locked: lock icon + dark grey 
            // Open: Bright grey
            // Open and Complete: ground grey

            Animator.LevelButtonsIn(menuBlocks.Select(b => b.gameObject));
        }

        public void SpawnLevel(Level level)
        {
            SpawnGroundBlocks(level);
            AnimateGroundBlocks(level);
            ColorGroundBlocks(level);

            SpawnMovableBlocks(level);
            AnimateMovableBlocks(level);

            highlights.ForEach(Destroy);
            highlights.Clear();
        }

        public void DespawnLevel()
        {
            movableBlocks.ForEach(block => Destroy(block.gameObject));
            groundBlocks.ForEach(block => Destroy(block.gameObject));
            highlights.ForEach(Destroy);
            menuBlocks.ForEach(Destroy);

            movableBlocks.Clear();
            groundBlocks.Clear();
            highlights.Clear();
            highlightLocations.Clear();
            menuBlocks.Clear();
        }

        private void SpawnGroundBlocks(Level level)
        {
            ConvertMenuBlocksToGround();

            var have = groundBlocks.Count;
            var need = level.groundBlocks.Count;

            if (have == need)
                return;

            if (have > need)
            {
                var amountDiscard = have - need;
                var blocksToDiscard = groundBlocks.Take(amountDiscard).ToList();
                foreach (var groundBlock in blocksToDiscard)
                {
                    groundBlocks.Remove(groundBlock);
                    var position = groundBlock.transform.position;
                    var zPosition = position.z + 10f;
                    Animator.Move(groundBlock.gameObject, position.With(z: zPosition), Type.Cardinal);
                    Destroy(groundBlock.gameObject, Animator.MoveTime);
                }
            }
            else
            {
                var amountAdd = need - have;
                For.Range(amountAdd, i => InstantiateGroundBlock(level.groundBlocks[i]));
            }
        }

        private void InstantiateGroundBlock(Location location)
        {
            var position = location.asVector3.With(y: -1);
            var groundBlock = Instantiate(groundPrefab, position, Quaternion.identity);
            groundBlock.location = location;
            groundBlocks.Add(groundBlock);
        }

        private void ConvertMenuBlocksToGround()
        {
            menuBlocks.ForEach(b =>
            {
                if (b.TryGetComponent<LevelButton>(out var levelButton))
                {
                    levelButton.HideImage();
                    Destroy(levelButton);
                }
            });
            groundBlocks.AddRange(menuBlocks.Select(b => b.GetOrAdd<GroundBlock>()));
            menuBlocks.Clear();
        }

        private void SpawnMovableBlocks(Level level) => For.Each<Type>(t => SpawnMovableBlocksOfType(t, level));

        private void SpawnMovableBlocksOfType(Type type, Level level)
        {
            var neededBlocks = level.startingConfiguration.Where(b => b.type == type).ToList();
            var have = movableBlocks.Count(b => b.model.type == type);
            var need = level.startingConfiguration.Count(b => b.type == type);

            if (have == need)
                return;

            if (have > need)
            {
                var amountDiscard = have - need;
                var blocksToDiscard = movableBlocks.Where(b => b.model.type == type).Take(amountDiscard).ToList();
                foreach (var block in blocksToDiscard)
                {
                    movableBlocks.Remove(block);
                    var position = block.transform.position;
                    var zPosition = position.z + 10f;
                    Animator.Move(block.gameObject, position.With(z: zPosition), Type.Cardinal);
                    Destroy(block.gameObject, Animator.MoveTime);
                }
            }
            else
            {
                var amountAdd = need - have;
                For.Range(amountAdd, i => InstantiateMovableBlock(neededBlocks[i]));
            }
        }

        private void InstantiateMovableBlock(Block block)
        {
            var prefab = block.type switch
            {
                Type.Cardinal => cardinalPrefab,
                Type.Diagonal => diagonalPrefab,
                Type.Frog => frogPrefab,
                _ => throw new ArgumentOutOfRangeException()
            };

            var position = block.location.asVector3;
            position += Random.onUnitSphere * 8;
            var movableBlock = Instantiate(prefab, position, Quaternion.Euler(-90, 0, 0));
            movableBlock.model = block;
            movableBlocks.Add(movableBlock);
        }

        private void AnimateMovableBlocks(Level level)
        {
            For.Each<Type>(t => AnimateMovableOfType(t, level));
        }

        private void AnimateMovableOfType(Type type, Level level)
        {
            var movables = movableBlocks
                .Where(b => b.model.type == type)
                .ToList();

            var startingBlocks =
                level.startingConfiguration
                    .Where(b => b.type == type)
                    .ToList();

            void SetBlock(int i)
            {
                movables[i].model = startingBlocks[i];
                Animator.Move(movables[i].gameObject, startingBlocks[i].location.asVector3.With(y: -0.35f),
                    Type.Cardinal);
            }

            For.Range(movables.Count, SetBlock);
        }

        private void AnimateGroundBlocks(Level level)
        {
            void SetBlock(int i)
            {
                var groundBlock = groundBlocks[i];
                var location = level.groundBlocks[i];
                groundBlock.location = location;

                Animator.Move(groundBlock.gameObject, location.asVector3.With(y: -1), Type.Cardinal);
            }

            For.Range(groundBlocks.Count, SetBlock);
        }

        private void ColorGroundBlocks(Level level)
        {
            foreach (var groundBlock in groundBlocks)
            {
                groundBlock.GetComponent<MeshRenderer>().material = Resources.Load<Material>($"Materials/Ground");
            }


            foreach (var block in level.targetConfiguration)
            {
                if (groundBlocks.FirstOption(g => g.location == block.location).IsSome(out var ground))
                {
                    ground.GetComponent<MeshRenderer>().material = block.material;
                }
            }
        }
    }
}