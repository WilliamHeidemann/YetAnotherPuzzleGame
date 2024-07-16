using UnityEngine;

namespace Animation
{
    public class FallAndBounceAnimation : AnimationData
    {
        private readonly Vector3 targetPosition;
        private readonly float fallTime;

        public FallAndBounceAnimation(GameObject gameObject, Vector3 targetPosition, float fallTime) : base(gameObject)
        {
            this.targetPosition = targetPosition;
            this.fallTime = fallTime;
        }

        public override LTSeq Tween()
        {
            var sequence = LeanTween.sequence();
            var fall = LeanTween.move(gameObject, targetPosition, fallTime).setEase(LeanTweenType.easeOutBounce);
            sequence.append(fall);
            return sequence;
        }
    }
}