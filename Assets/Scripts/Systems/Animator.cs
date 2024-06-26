using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using UnityEngine;

namespace Systems
{
    public static class Animator
    {
        private static readonly Dictionary<GameObject, Queue<Vector3>> Animations = new();
        private const float FadeTime = 2f;
        private const float MoveTime = 1f;

        public static void Move(GameObject objectToMove, Vector3 targetLocation, Type moveType)
        {
            if (!Animations.ContainsKey(objectToMove)) Animations.Add(objectToMove, new Queue<Vector3>());
            if (Animations[objectToMove].Count == 0)
            {
                CreateTween(objectToMove, targetLocation);
            }

            Animations[objectToMove].Enqueue(targetLocation);

            void CreateTween(GameObject o, Vector3 position)
            {
                var tween = LeanTween.move(o, position, MoveTime).setEase(LeanTweenType.easeOutQuad);
                tween.setOnComplete(() => StartNext(o));
            }

            void StartNext(GameObject obj)
            {
                Animations[obj].Dequeue();
                if (!Animations[obj].TryPeek(out var next)) return;
                CreateTween(objectToMove, next);
            }
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
    }
}