using UnityEngine;

namespace Animation
{
    public class FrogAnimation : AnimationData
    {
        private readonly Vector3 targetPosition;
        private const float JumpHeight = 2f;
        private const float JumpDuration = 1f;
        private const float CrouchDuration = 0.2f;


        public FrogAnimation(GameObject gameObject, Vector3 targetPosition) : base(gameObject)
        {
            this.targetPosition = targetPosition;
        }

        public LTSeq Tween2()
        {
            // Create a LeanTween sequence
            var sequence = LeanTween.sequence();

            // Calculate jump peak position
            var jumpPeak = (gameObject.transform.position + targetPosition) / 2 + Vector3.up * JumpHeight;

            // Crouch down
            sequence.append(LeanTween.scaleY(gameObject, 0.2f, CrouchDuration).setEase(LeanTweenType.easeInQuad));

            // Jump up
            sequence.append(LeanTween.scaleY(gameObject, 0.6f, CrouchDuration).setEase(LeanTweenType.easeOutBounce)
                // Move to jump peak
                .setOnStart(() =>
                    LeanTween.move(gameObject, jumpPeak, JumpDuration / 2))); //.setEase(LeanTweenType.easeOutQuad)));


            // Move to target position
            sequence.append(LeanTween.move(gameObject, targetPosition, JumpDuration / 2));
            //.setEase(LeanTweenType.easeInQuad));

            // Land
            sequence.append(LeanTween.scaleY(gameObject, 0.2f, CrouchDuration).setEase(LeanTweenType.easeOutQuad));

            // Return to normal state
            sequence.append(LeanTween.scaleY(gameObject, 0.6f, CrouchDuration / 2).setEase(LeanTweenType.easeInQuad));

            return sequence;
        }

        // translate from start to middle AND from down to up to down AND rotate 180 degrees
        // then append
        // the same again
        public override LTSeq Tween()
        {
            var sequence = LeanTween.sequence();
            var middle = (gameObject.transform.position + targetPosition) / 2 + Vector3.up;
            
            sequence.insert(LeanTween.moveX(gameObject, middle.x, JumpDuration));
            sequence.insert(LeanTween.moveZ(gameObject, middle.z, JumpDuration));
            sequence.insert(LeanTween.moveY(gameObject, JumpHeight, JumpDuration / 2)
                .setOnComplete(() => LeanTween.moveY(gameObject, 0f, JumpDuration / 2)));
            sequence.append(LeanTween.rotateZ(gameObject, 180, JumpDuration));

            sequence.insert(LeanTween.moveX(gameObject, targetPosition.x, JumpDuration));
            sequence.insert(LeanTween.moveZ(gameObject, targetPosition.z, JumpDuration));
            sequence.insert(LeanTween.moveY(gameObject, JumpHeight, JumpDuration / 2)
                .setOnComplete(() => LeanTween.moveY(gameObject, 0f, JumpDuration / 2)));
            sequence.append(LeanTween.rotateZ(gameObject, 360, JumpDuration));

            return sequence;
        }
    }
}