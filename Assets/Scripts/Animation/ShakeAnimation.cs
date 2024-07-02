using UnityEngine;

namespace Animation
{
    public class ShakeAnimation : AnimationData
    {
        public ShakeAnimation(GameObject gameObject) : base(gameObject)
        {
        }

        public override LTDescr Tween()
        {
            // Create a new sequence
            LTSeq shakeSequence = LeanTween.sequence();

            // Define the shaking duration and intensity
            float duration = 1.0f;
            float intensity = 10f; // Adjust this value to control the shake intensity

            // Add rotation tweens to the sequence
            shakeSequence.append(LeanTween.rotateZ(gameObject, intensity, duration / 4)
                .setEase(LeanTweenType.easeInOutSine));
            shakeSequence.append(LeanTween.rotateZ(gameObject, -intensity, duration / 2)
                .setEase(LeanTweenType.easeInOutSine));
            shakeSequence.append(
                LeanTween.rotateZ(gameObject, 0, duration / 4).setEase(LeanTweenType.easeInOutSine));

            // Optionally, you can repeat the sequence
            // shakeSequence.setLoopPingPong(1); // Repeat the shake sequence once (you can adjust the loop count as needed)

            return shakeSequence.tween;
        }
    }
    
}