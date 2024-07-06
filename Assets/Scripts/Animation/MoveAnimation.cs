using Model;
using UnityEngine;

namespace Animation
{
    public class MoveAnimation : AnimationData
    {
        private readonly Vector3 destination;

        public MoveAnimation(GameObject gameObject, Vector3 destination) : base(gameObject)
        {
            this.destination = destination;
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