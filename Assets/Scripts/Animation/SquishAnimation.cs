using UnityEngine;

namespace Animation
{
    public class SquishAnimation : AnimationData
    {
        private const float SquishScale = 0.8f;
        private const float SquishDuration = .1f;
        private const float RaiseDuration = .3f;

        private readonly float originalScaleY = 1f;
        private readonly float originalPositionY = 0f;

        public SquishAnimation(GameObject gameObject) : base(gameObject)
        {
            // originalScaleY = gameObject.transform.localScale.y;
            // originalPositionY = gameObject.transform.localPosition.y;
            Debug.Log($"original scale y: {originalScaleY}");
            Debug.Log($"original position y: {originalPositionY}");
        }

        public override LTDescr Tween()
        {
            var targetScaleY = originalScaleY * SquishScale;
            var targetPositionY = originalPositionY - (originalScaleY - targetScaleY) / 2f;
            Debug.Log($"target scale y: {targetScaleY}");
            Debug.Log($"target position y: {targetPositionY}");

            return LeanTween.scaleY(gameObject, targetScaleY, SquishDuration)
                .setEaseOutCubic()
                .setOnStart(() =>
                {
                    // Adjust position for downward scaling effect
                    LeanTween.moveY(gameObject, targetPositionY, SquishDuration)
                        .setEaseOutCubic();
                })
                .setOnComplete(() =>
                {
                    // Raise back up animation
                    LeanTween.scaleY(gameObject, originalScaleY / targetScaleY, RaiseDuration)
                        .setEaseOutBounce()
                        .setOnStart(() =>
                        {
                            // Adjust position for downward scaling effect
                            LeanTween.moveY(gameObject, originalPositionY / targetPositionY, RaiseDuration)
                                .setEaseOutBounce();
                        });
                });
        }
    }
}