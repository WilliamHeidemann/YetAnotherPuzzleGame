using UnityEngine;

namespace Animation
{
    public class ButtonShowAnimation : AnimationData
    {
        private readonly float originalScaleY = 0.9f;
        private readonly float originalPositionY = -1f;
        
        private const float SquishScale = 0.2f;
        private const float SquishDuration = .3f;

        
        public ButtonShowAnimation(GameObject gameObject) : base(gameObject)
        {
        }

        public override LTSeq Tween()
        {
            var downTween = LeanTween.scaleY(gameObject, originalScaleY, SquishDuration)
                .setEaseOutBounce()
                .setOnStart(() =>
                {
                    LeanTween.moveY(gameObject, originalPositionY, SquishDuration)
                        .setEaseOutBounce();
                });
            
            
            var sequence = LeanTween.sequence();
            sequence.append(downTween);
            return sequence;
        }
    }
}