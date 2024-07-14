using Systems;
using UnityEngine;

namespace Components
{
    public class WorldButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private LevelManager.World world;
        public void Interact()
        {
            LevelManager.Instance.SetWorld(world);
            MainMenu.Instance.onWorldSelected?.Invoke();
        }
    }
}
