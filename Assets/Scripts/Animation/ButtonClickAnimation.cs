using System;
using UnityEngine;

namespace Animation
{
    public class ButtonClickAnimation : AnimationData
    {
        private const float SquishScale = 0.2f;
        private const float SquishDuration = .1f;
        private const float RaiseDuration = .3f;

        private readonly float originalScaleY = 0.9f;
        private readonly float originalPositionY = -1f;

        public ButtonClickAnimation(GameObject gameObject) : base(gameObject)
        {
            // originalScaleY = gameObject.transform.localScale.y;
            // originalPositionY = gameObject.transform.localPosition.y;
        }

        public override LTSeq Tween()
        {
            var targetScaleY = originalScaleY * SquishScale;
            var targetPositionY = originalPositionY - (originalScaleY - targetScaleY) / 2f;

            var downTween = LeanTween.scaleY(gameObject, targetScaleY, SquishDuration)
                .setEaseOutCubic()
                .setOnStart(() =>
                {
                    LeanTween.moveY(gameObject, targetPositionY, SquishDuration)
                        .setEaseOutCubic();
                });

            var upTween = LeanTween.scaleY(gameObject, originalScaleY, RaiseDuration)
                .setEaseOutBounce()
                .setOnStart(() =>
                {
                    LeanTween.moveY(gameObject, originalPositionY, RaiseDuration)
                        .setEaseOutBounce();
                });

            var sequence = LeanTween.sequence();
            sequence.append(downTween);
            sequence.append(upTween);
            return sequence;
        }
    }
}