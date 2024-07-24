using Model;
using Systems;
using UnityEngine;

namespace Animation
{
    public class MoveAnimation : AnimationData
    {
        private readonly Vector3 destination;
        private readonly bool withSound;

        public MoveAnimation(GameObject gameObject, Vector3 destination, bool withSound) : base(gameObject)
        {
            this.destination = destination;
            this.withSound = withSound;
        }

        public override LTSeq Tween()
        {
            var sequence = LeanTween.sequence();
            if (gameObject == null) return sequence;
            var move = LeanTween.move(gameObject, destination, Animator.MoveTime).setEase(LeanTweenType.easeOutQuad);
            if (withSound)
                sequence.append(() => SoundEffectSystem.Instance.PlayMove());
            sequence.append(move);
            return sequence;
        }
    }
}