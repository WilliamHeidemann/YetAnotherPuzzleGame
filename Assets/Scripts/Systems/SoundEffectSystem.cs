using UnityEngine;
using UnityEngine.Serialization;
using UnityUtils;

namespace Systems
{
    public class SoundEffectSystem : Singleton<SoundEffectSystem>
    {
        [SerializeField] private AudioClip slide;
        [SerializeField] private AudioClip bubble;
        [SerializeField] private AudioClip wood;
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
            audioSource.PlayOneShot(slide);
        }

        public void PlayWood()
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(wood);
        }

        public void PlayBubble()
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(bubble);
        }
    }
}
