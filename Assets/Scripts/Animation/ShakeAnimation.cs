using UnityEngine;

namespace Animation
{
    public class ShakeAnimation : AnimationData
    {
        public ShakeAnimation(GameObject gameObject) : base(gameObject)
        {
        }

        public override LTSeq Tween()
        {
            const float duration = 1.0f;
            const float intensity = 10f;

            var shakeSequence = LeanTween.sequence();
            shakeSequence.append(LeanTween.rotateY(gameObject, intensity, duration / 4)
                .setEase(LeanTweenType.easeInOutSine));
            shakeSequence.append(LeanTween.rotateY(gameObject, -intensity, duration / 2)
                .setEase(LeanTweenType.easeInOutSine));
            shakeSequence.append(LeanTween.rotateY(gameObject, 0, duration / 4)
                .setEase(LeanTweenType.easeInOutSine));

            return shakeSequence;
        }
    }
}