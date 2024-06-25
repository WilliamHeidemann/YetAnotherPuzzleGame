using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;

namespace Systems
{
    public static class Animator
    {
        private static readonly Dictionary<GameObject, Queue<Vector3>> Animations = new();
        private const float FadeTime = 2f;
        private const float MoveTime = 1f;

        // Metode kald fortæller kun hvilken animations næste gang der bliver tid. Altså sætter den ikke igang.
        // Classen selv bestemmer hvornår disse bliver afspillet.

        // One list for objects to move
        // One list for objects to fade out
        // One list for objects to fade in

        private static readonly List<GameObject> BlocksBeingMoved = new();
        private static readonly List<GameObject> BlocksToFadeOut = new();
        private static readonly List<GameObject> BlocksToFadeIn = new();

        public static void  HandleAnimations()
        {
            if (BlocksBeingMoved.Count > 0) return;
            if (BlocksToFadeOut.Count > 0) FadeBlocksOut();
            if (BlocksToFadeIn.Count > 0) FadeBlocksIn();
        }

        public static void Move(GameObject objectToMove, Vector3 targetLocation, Type moveType)
        {
            if (!Animations.ContainsKey(objectToMove)) Animations.Add(objectToMove, new Queue<Vector3>());
            if (Animations[objectToMove].Count == 0)
            {
                CreateTween(objectToMove, targetLocation);
            }

            Animations[objectToMove].Enqueue(targetLocation);
            BlocksBeingMoved.Add(objectToMove);

            void CreateTween(GameObject o, Vector3 position)
            {
                var tween = LeanTween.move(o, position, MoveTime).setEase(LeanTweenType.easeOutQuad);
                tween.setOnComplete(() => StartNext(o));
            }

            void StartNext(GameObject obj)
            {
                BlocksBeingMoved.Remove(obj);
                Animations[obj].Dequeue();
                if (!Animations[obj].TryPeek(out var next)) return;
                CreateTween(objectToMove, next);
            }
        }


        public static void BlocksOut(IEnumerable<GameObject> blocks)
        {
            BlocksToFadeOut.AddRange(blocks);

        }

        private static void FadeBlocksOut()
        {
            foreach (var block in BlocksToFadeOut)
            {
                var distance = (Random.value + 1) * 10;
                var point = Random.insideUnitSphere * distance;
                LeanTween.move(block, point, FadeTime).setEase(LeanTweenType.easeOutQuad);
                LeanTween.scale(block, Vector3.zero, FadeTime).setEase(LeanTweenType.easeInQuad)
                    .setOnComplete(() => BlocksToFadeOut.Remove(block));
            }
        }

        public static void BlocksIn(IEnumerable<GameObject> blocks)
        {
            BlocksToFadeIn.AddRange(blocks);
        }

        private static void FadeBlocksIn()
        {
            foreach (var block in BlocksToFadeIn)
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

            BlocksToFadeIn.Clear();
        }
    }
}