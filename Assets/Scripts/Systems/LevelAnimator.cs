using System.Collections.Generic;
using UnityEngine;

namespace Systems
{
    public static class LevelAnimator
    {
        public static void BlocksOut(IEnumerable<GameObject> blocks)
        {
            foreach (var block in blocks)
            {
                var distance = (Random.value + 1) * 10;
                var point = Random.insideUnitSphere * distance;
                LeanTween.move(block, point, 2).setEase(LeanTweenType.easeOutQuad);
                LeanTween.scale(block, Vector3.zero, 2).setEase(LeanTweenType.easeInQuad);
            }
        }

        public static void BlocksIn(IEnumerable<GameObject> blocks)
        {
            foreach (var block in blocks)
            {
                var originalPosition = block.transform.position;
                var distance = (Random.value + 1) * 10;
                var point = Random.insideUnitSphere * distance;
                block.transform.position = point;
                
                var originalScale = block.transform.lossyScale;
                block.transform.localScale = Vector3.zero;

                LeanTween.move(block, originalPosition, 2).setEase(LeanTweenType.easeOutQuad);
                LeanTween.scale(block, originalScale, 2).setEase(LeanTweenType.easeInQuad);
            }
        }
    }
}