using UnityEngine;

namespace Animation
{
    public class ButtonHideAnimation : AnimationData
    {
        private readonly float originalScaleY = 0.9f;
        private readonly float originalPositionY = -1f;
        
        private const float SquishScale = 0.2f;
        private const float SquishDuration = .3f;

        
        public ButtonHideAnimation(GameObject gameObject) : base(gameObject)
        {
        }

        public override LTSeq Tween()
        {
            var targetScaleY = originalScaleY * SquishScale;
            var targetPositionY = originalPositionY - (originalScaleY - targetScaleY) / 2f;

            var downTween = LeanTween.scaleY(gameObject, targetScaleY, SquishDuration)
                .setEaseOutBounce()
                .setOnStart(() =>
                {
                    LeanTween.moveY(gameObject, targetPositionY, SquishDuration)
                        .setEaseOutBounce();
                });
            
            
            var sequence = LeanTween.sequence();
            sequence.append(downTween);
            return sequence;
        }
    }
}