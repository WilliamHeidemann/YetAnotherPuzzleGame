using Systems;
using UnityUtils;
using Animator = Animation.Animator;

namespace Components
{
    public class UndoButton : Singleton<UndoButton>, IInteractable
    {
        private bool isActive;

        public void Interact()
        {
            if (!isActive)
                return;
            Animator.ButtonHide(gameObject);
            Selector.Instance.Undo();
        }

        public void SetActive(bool active)
        {
            if (active)
            {
                Animator.ButtonShow(gameObject);
            }
            else
            {
                Animator.ButtonHide(gameObject);
            }

            isActive = active;
        }
    }
}