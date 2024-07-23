using UnityEngine;
using UnityUtils;

namespace Systems
{
    public class SoundEffectSystem : Singleton<SoundEffectSystem>
    {
        [SerializeField] private AudioClip movePop;
        [SerializeField] private AudioClip undoPop;
        [SerializeField] private AudioSource audioSource;
        private const float PitchIncrement = 0.35f;
        private float pitch;
        [SerializeField] private GameObject slash;
        private bool isEnabled = true;

        public void ToggleSoundEffects()
        {
            isEnabled = !isEnabled;
            audioSource.volume = isEnabled ? 1f : 0f;
            slash.gameObject.SetActive(!isEnabled);
        }
        
        public void PlayMove()
        {
            pitch = (pitch + PitchIncrement) % 0.4f + 1;
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(movePop);
        }

        public void PlayUndo()
        {
            audioSource.PlayOneShot(undoPop);
        }
    }
}
