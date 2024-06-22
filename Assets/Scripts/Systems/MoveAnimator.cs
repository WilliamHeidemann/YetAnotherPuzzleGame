using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public class MoveAnimator
    {
        private readonly Dictionary<GameObject, Queue<Vector3>> animations = new();

        private void Tween(GameObject objectToMove, Vector3 targetLocation)
        {
            if (!animations.ContainsKey(objectToMove)) animations.Add(objectToMove, new Queue<Vector3>());
            if (animations[objectToMove].Count == 0)
            {
                CreateTween(objectToMove, targetLocation);
            }

            animations[objectToMove].Enqueue(targetLocation);

            void CreateTween(GameObject o, Vector3 position)
            {
                var tween = LeanTween.move(o, position, 1f).setEase(LeanTweenType.easeOutQuad);
                tween.setOnComplete(() => StartNext(o));
            }

            void StartNext(GameObject obj)
            {
                animations[obj].Dequeue();
                if (!animations[obj].TryPeek(out var next)) return;
                CreateTween(objectToMove, next);
            }
        }
    }
}