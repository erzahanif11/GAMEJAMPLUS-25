using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip menuMusic;
    public AudioClip gameMusic;

    public AudioClip menuSelect;
    public AudioClip settingSelectSound;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip dead;
    public AudioClip hit;
    public AudioClip pickupCoin;
    public AudioClip powerUp;
    public AudioClip portal;
    public AudioClip breakBlock;

    private void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if(currentScene == "Main Menu")
        {
            PlayMenuMusic();
        }
        else if (currentScene == "Main Scene")
        {
            PlayGameMusic();
        }
    }

    public void PlayMenuMusic()
    {
        musicSource.clip = menuMusic;
        musicSource.Play();
    }

    public void PlayGameMusic()
    {
        musicSource.clip = gameMusic;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
