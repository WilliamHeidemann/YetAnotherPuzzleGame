using UnityEngine;
using UnityEngine.UI;

namespace Systems
{
    public class MusicSystem : MonoBehaviour
    {
        [SerializeField] private AudioSource music;
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
            music.volume = volume;
            slash.SetActive(volume == 0f);
        }
    }
}