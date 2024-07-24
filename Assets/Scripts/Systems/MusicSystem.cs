using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    [SerializeField] private AudioSource music;
    [SerializeField] private GameObject slash;
    public void ToggleMusic()
    {
        if (music.isPlaying) 
            music.Pause();
        else 
            music.UnPause();
        
        slash.SetActive(!music.isPlaying);
    }
}
