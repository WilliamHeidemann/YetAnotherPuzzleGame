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

        public override LTDescr Tween() =>
            LeanTween.move(gameObject, destination, Animator.MoveTime).setEase(LeanTweenType.easeOutQuad);
    }
}