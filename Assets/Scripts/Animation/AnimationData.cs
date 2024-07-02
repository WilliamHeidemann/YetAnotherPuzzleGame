using UnityEngine;

namespace Animation
{
    public abstract class AnimationData
    {
        public readonly GameObject gameObject;

        protected AnimationData(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        // Side effect: Starts playing an animation.
        public abstract LTDescr Tween();
    }
}