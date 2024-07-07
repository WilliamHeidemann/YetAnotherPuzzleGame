using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Components;
using Model;
using UnityEngine;
using Random = UnityEngine.Random;
using Type = Model.Type;

namespace Animation
{
    public static class Animator
    {
        private static readonly Dictionary<GameObject, Queue<AnimationData>> Animations = new();
        private const float FadeTime = 2f;
        public const float MoveTime = 1f;

        public static void Move(GameObject obj, Vector3 targetLocation, Type moveType)
        {
            switch (moveType)
            {
                case Type.Frog:
                    // QueueAnimation(new FrogAnimation(obj, targetLocation));
                    // break;
                case Type.Cardinal:
                case Type.Diagonal:
                default:
                    QueueAnimation(new MoveAnimation(obj, targetLocation));
                    break;
            }
        }

        public static void Shake(GameObject obj)
        {
            QueueAnimation(new ShakeAnimation(obj));
        }

        public static void ButtonClick(GameObject obj)
        {
            QueueAnimation(new ButtonClickAnimation(obj));
        }

        public static void ButtonHide(GameObject obj)
        {
            QueueAnimation(new ButtonHideAnimation(obj));
        }
        
        public static void ButtonShow(GameObject obj)
        {
            QueueAnimation(new ButtonShowAnimation(obj));
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
            else if (queueLength == 3)
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

        public static async Task BlocksOut(IEnumerable<GameObject> blocks)
        {
            while (Animations.Values.Any(queue => queue.Count > 0))
            {
                await Awaitable.NextFrameAsync();
            }

            Animations.Clear();

            foreach (var block in blocks)
            {
                var distance = (Random.value + 1) * 10;
                var point = Random.insideUnitSphere * distance;
                LeanTween.move(block, point, FadeTime).setEase(LeanTweenType.easeOutQuad);
                LeanTween.scale(block, Vector3.zero, FadeTime).setEase(LeanTweenType.easeInQuad);
            }

            await Awaitable.WaitForSecondsAsync(FadeTime);
        }

        public static async Task BlocksIn(IEnumerable<GameObject> blocks)
        {
            foreach (var block in blocks)
            {
                var originalPosition = block.transform.position;
                var distance = (Random.value + 1) * 10;
                var point = Random.insideUnitSphere * distance;
                block.transform.position = point;

                var originalScale = block.transform.lossyScale;
                block.transform.localScale = Vector3.zero;

                LeanTween.move(block, originalPosition, FadeTime).setEase(LeanTweenType.easeOutQuad);
                LeanTween.scale(block, originalScale, FadeTime).setEase(LeanTweenType.easeInQuad);
            }

            await Awaitable.WaitForSecondsAsync(FadeTime);
        }

        public static async Task ResetLevel(IEnumerable<Block> levelStartingConfiguration,
            IEnumerable<MovableBlock> movableBlocks)
        {
            while (Animations.Values.Any(queue => queue.Count > 0))
            {
                await Awaitable.NextFrameAsync();
            }

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
                Move(movable.gameObject, block.location.asVector3, block.type);
            }

            await Awaitable.WaitForSecondsAsync(MoveTime);
        }
    }
}