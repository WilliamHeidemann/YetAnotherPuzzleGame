using Systems;
using UnityEngine;
using Animator = Animation.Animator;

namespace Components
{
    public class RewindButton : MonoBehaviour
    {
        public void Spin()
        {
            Animator.Spin(gameObject);
        }
    }
}