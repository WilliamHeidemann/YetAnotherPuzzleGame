using Systems;
using UnityEngine;

namespace Animation
{
    public class FrogAnimation : AnimationData
    {
        private readonly Vector3 targetPosition;
        private readonly bool withSound;

        public FrogAnimation(GameObject gameObject, Vector3 targetPosition, bool withSound) : base(gameObject)
        {
            this.targetPosition = targetPosition;
            this.withSound = withSound;
        }

        public override LTSeq Tween()
        {
            var middle = (gameObject.transform.position + targetPosition) / 2 + Vector3.up;

            var sequence = LeanTween.sequence();
            if (withSound)
                sequence.append(() => SoundEffectSystem.Instance.PlayMove());
            sequence.append(() =>
            {
                LeanTween.moveX(gameObject, middle.x, Animator.MoveTime / 2).setEase(LeanTweenType.easeInQuad);
                LeanTween.moveZ(gameObject, middle.z, Animator.MoveTime / 2).setEase(LeanTweenType.easeInQuad);
            });
            sequence.append(LeanTween.moveY(gameObject, middle.y, Animator.MoveTime / 2)
                .setEase(LeanTweenType.easeOutSine));
            
            sequence.append(() =>
            {
                LeanTween.moveX(gameObject, targetPosition.x, Animator.MoveTime / 2).setEase(LeanTweenType.easeOutQuad);
                LeanTween.moveZ(gameObject, targetPosition.z, Animator.MoveTime / 2).setEase(LeanTweenType.easeOutQuad);
            });
            
            sequence.append(LeanTween.moveY(gameObject, targetPosition.y, Animator.MoveTime / 2)
                .setEase(LeanTweenType.easeInSine));

            return sequence;
        }
    }
}