using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
using Type = Model.Type;

namespace Systems
{
    public static class Animator
    {
        private static readonly Dictionary<GameObject, Queue<AnimationData>> Animations = new();
        private const float FadeTime = 2f;
        private const float MoveTime = 1f;

        public static void Move(GameObject objectToMove, Vector3 targetLocation, Type moveType)
        {
            QueueAnimation(new MoveAnimation(objectToMove, targetLocation, moveType));
        }

        public static void Shake(GameObject objectToMove)
        {
            QueueAnimation(new ShakeAnimation(objectToMove));
        }

        private static void QueueAnimation(AnimationData animation)
        {
            if (!Animations.ContainsKey(animation.gameObject))
            {
                Animations.Add(animation.gameObject, new Queue<AnimationData>());
            }

            if (Animations[animation.gameObject].Count == 0)
            {
                CreateTween(animation);
            }

            Animations[animation.gameObject].Enqueue(animation);
        }
        
        private static void CreateTween(AnimationData animation)
        {
            animation.Tween().setOnComplete(() => StartNextAnimation(animation.gameObject));
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

        private abstract class AnimationData
        {
            public readonly GameObject gameObject;

            public AnimationData(GameObject gameObject)
            {
                this.gameObject = gameObject;
            }

            // Side effect: Starts playing an animation.
            public abstract LTDescr Tween();
        }

        // Instead of passing a blockType, there might be a different animation class for each type. 
        private class MoveAnimation : AnimationData
        {
            public readonly Vector3 destination;
            public readonly Type blockType;

            public MoveAnimation(GameObject gameObject, Vector3 destination, Type type) : base(gameObject)
            {
                this.destination = destination;
                blockType = type;
            }

            public override LTDescr Tween() => 
                LeanTween.move(gameObject, destination, MoveTime).setEase(LeanTweenType.easeOutQuad);
        }

        private class ShakeAnimation : AnimationData
        {
            public ShakeAnimation(GameObject gameObject) : base(gameObject)
            {
            }

            public override LTDescr Tween()
            {
                // Create a new sequence
                LTSeq shakeSequence = LeanTween.sequence();

                // Define the shaking duration and intensity
                float duration = 1.0f;
                float intensity = 10f; // Adjust this value to control the shake intensity

                // Add rotation tweens to the sequence
                shakeSequence.append(LeanTween.rotateZ(gameObject, intensity, duration / 4).setEase(LeanTweenType.easeInOutSine));
                shakeSequence.append(LeanTween.rotateZ(gameObject, -intensity, duration / 2).setEase(LeanTweenType.easeInOutSine));
                shakeSequence.append(LeanTween.rotateZ(gameObject, 0, duration / 4).setEase(LeanTweenType.easeInOutSine));
        
                // Optionally, you can repeat the sequence
                // shakeSequence.setLoopPingPong(1); // Repeat the shake sequence once (you can adjust the loop count as needed)

                return shakeSequence.tween;
            }
        }
    }
}