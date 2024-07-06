using UnityEngine;

namespace Animation
{
    public class FrogAnimation : AnimationData
    {
        // The target position for the frog to jump to
        public Vector3 targetPosition;

        // Jump height
        public float jumpHeight = 3f;

        // Duration of the jump
        public float jumpDuration = 1f;

        // Duration of the crouch and land
        public float crouchDuration = 0.2f;


        public FrogAnimation(GameObject gameObject, Vector3 targetPosition) : base(gameObject)
        {
            this.targetPosition = targetPosition;
        }

        public override LTSeq Tween()
        {
            // Create a LeanTween sequence
            var sequence = LeanTween.sequence();

            // Calculate jump peak position
            var jumpPeak = (gameObject.transform.position + targetPosition) / 2 + Vector3.up * jumpHeight;

            // Crouch down
            sequence.append(LeanTween.scaleY(gameObject, 0.2f, crouchDuration).setEase(LeanTweenType.easeInQuad));

            // Jump up
            sequence.append(LeanTween.scaleY(gameObject, 0.6f, crouchDuration).setEase(LeanTweenType.easeOutBounce)
                // Move to jump peak
                .setOnStart(() =>
                    LeanTween.move(gameObject, jumpPeak, jumpDuration / 2)));//.setEase(LeanTweenType.easeOutQuad)));


            // Move to target position
            sequence.append(LeanTween.move(gameObject, targetPosition, jumpDuration / 2));
                //.setEase(LeanTweenType.easeInQuad));

            // Land
            sequence.append(LeanTween.scaleY(gameObject, 0.2f, crouchDuration).setEase(LeanTweenType.easeOutQuad));

            // Return to normal state
            sequence.append(LeanTween.scaleY(gameObject, 0.6f, crouchDuration / 2).setEase(LeanTweenType.easeInQuad));

            return sequence;
        }
    }
}