using UnityEngine;

namespace Animation
{
    public class SpinAnimation : AnimationData
    {
        public SpinAnimation(GameObject gameObject) : base(gameObject)
        {
        }

        public override LTSeq Tween()
        {
            var sequence = LeanTween.sequence();
            var spin = LeanTween.rotateAroundLocal(gameObject, Vector3.back, 360, 1f).setEase(LeanTweenType.easeInOutBack);
            sequence.append(spin);
            return sequence;
        }
    }
}