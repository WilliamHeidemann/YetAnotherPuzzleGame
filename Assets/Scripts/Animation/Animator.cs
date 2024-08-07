﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Model;
using Systems;
using UnityEngine;
using UnityUtils;
using Random = UnityEngine.Random;
using Type = Model.Type;

namespace Animation
{
    public static class Animator
    {
        private static readonly Dictionary<GameObject, Queue<AnimationData>> Animations = new();
        public const float MoveTime = 1f;

        public static void Move(GameObject obj, Vector3 targetLocation, Type moveType, bool withSound = false)
        {
            switch (moveType)
            {
                case Type.Frog:
                    QueueAnimation(new FrogAnimation(obj, targetLocation, withSound));
                    break;
                case Type.Cardinal:
                case Type.Diagonal:
                default:
                    QueueAnimation(new MoveAnimation(obj, targetLocation, withSound));
                    break;
            }
        }

        public static void Shake(GameObject obj)
        {
            QueueAnimation(new ShakeAnimation(obj));
        }

        public static void Spin(GameObject obj)
        {
            QueueAnimation(new SpinAnimation(obj));
        }

        public static void FallAndBounce(GameObject obj, Vector3 targetPosition, float fallTime)
        {
            QueueAnimation(new FallAndBounceAnimation(obj, targetPosition, fallTime));
        }

        public static void ClickTutorial(GameObject obj, TutorialSystem.TutorialData data)
        {
            QueueAnimation(new ClickTutorialAnimation(obj, data));
        }
        
        public static void ClearQueue(GameObject obj)
        {
            if (Animations.TryGetValue(obj, out var queue))
            {
                queue.Clear();
            }   
        }

        private static void QueueAnimation(AnimationData animation)
        {
            if (!Animations.ContainsKey(animation.gameObject))
            {
                Animations.Add(animation.gameObject, new Queue<AnimationData>());
            }

            var queueLength = Animations[animation.gameObject].Count;
            if (queueLength == 0)
            {
                CreateTween(animation);
            }
            else if (queueLength == 3 && animation is not MoveAnimation or FrogAnimation)
            {
                return;
            }

            Animations[animation.gameObject].Enqueue(animation);
        }

        private static void CreateTween(AnimationData animation)
        {
            animation.Tween().append(() => StartNextAnimation(animation.gameObject));
        }

        private static void StartNextAnimation(GameObject obj)
        {
            Animations[obj].Dequeue();
            if (!Animations[obj].TryPeek(out var next)) return;
            CreateTween(next);
        }

        public static async Task ResetLevel(IEnumerable<Block> levelStartingConfiguration,
            IEnumerable<MovableBlock> movableBlocks)
        {
            while (Animations.Values.Any(queue => queue.Count > 0))
            {
                await Awaitable.NextFrameAsync();
            }
            
            Spawner.Instance.HideHighlights();

            var movableList = new List<MovableBlock>(movableBlocks);
            var startPositions = levelStartingConfiguration.ToList();

            var atStart = movableList.Where(b => startPositions.Contains(b.model)).ToList();
            foreach (var movableBlock in atStart)
            {
                movableList.Remove(movableBlock);
                startPositions.Remove(movableBlock.model);
            }

            foreach (var block in startPositions)
            {
                var movable = movableList.First(b => b.model.type == block.type);
                movable.model = block;
                movableList.Remove(movable);
                Move(movable.gameObject, block.location.asVector3.With(y: -0.35f), block.type);
            }

            await Awaitable.WaitForSecondsAsync(MoveTime);
        }

        public static void LevelButtonsIn(IEnumerable<GameObject> levelButtons)
        {
            foreach (var levelButton in levelButtons)
            {
                var fallTime = Random.Range(.6f, 2f);
                var targetPosition = levelButton.transform.position;
                levelButton.transform.position = targetPosition.With(y: 20);
                // LeanTween.move(levelButton, targetPosition, fallTime).setEase(LeanTweenType.easeOutBounce);
                
                FallAndBounce(levelButton, targetPosition, fallTime);
            }
        }
    }
}