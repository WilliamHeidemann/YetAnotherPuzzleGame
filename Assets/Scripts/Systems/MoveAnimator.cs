using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public static class MoveAnimator
    {
        private static readonly Dictionary<GameObject, Queue<Vector3>> Animations = new();

        public static void Move(GameObject objectToMove, Vector3 targetLocation)
        {
            if (!Animations.ContainsKey(objectToMove)) Animations.Add(objectToMove, new Queue<Vector3>());
            if (Animations[objectToMove].Count == 0)
            {
                CreateTween(objectToMove, targetLocation);
            }

            Animations[objectToMove].Enqueue(targetLocation);

            void CreateTween(GameObject o, Vector3 position)
            {
                var tween = LeanTween.move(o, position, 1f).setEase(LeanTweenType.easeOutQuad);
                tween.setOnComplete(() => StartNext(o));
            }

            void StartNext(GameObject obj)
            {
                Animations[obj].Dequeue();
                if (!Animations[obj].TryPeek(out var next)) return;
                CreateTween(objectToMove, next);
            }
        }

        public static void Clear() => Animations.Clear();
    }
}