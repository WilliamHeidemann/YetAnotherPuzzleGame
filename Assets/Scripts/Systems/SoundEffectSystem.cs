using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
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

        [SerializeField] private Slider slider;
        private bool isSliderActive;
        
        private void Start()
        {
            slider.onValueChanged.AddListener(AdjustMusicVolume);
        }

        public void ToggleSlider()
        {
            isSliderActive = !isSliderActive;
            slider.gameObject.SetActive(isSliderActive);
        }

        public void HideSlider()
        {
            isSliderActive = false;
            slider.gameObject.SetActive(false);
        }

        private void AdjustMusicVolume(float volume)
        {
            audioSource.volume = volume;
            slash.SetActive(volume == 0f);
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
