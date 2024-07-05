using Model;
using UnityEngine;

namespace Animation
{
    public class MoveAnimation : AnimationData
    {
        private readonly Vector3 destination;
        private readonly Type blockType;

        public MoveAnimation(GameObject gameObject, Vector3 destination, Type type) : base(gameObject)
        {
            this.destination = destination;
            blockType = type;
        }

        public override LTSeq Tween()
        {
            var sequence = LeanTween.sequence();
            var move = LeanTween.move(gameObject, destination, Animator.MoveTime).setEase(LeanTweenType.easeOutQuad);
            sequence.append(move);
            return sequence;
        }
    }
}