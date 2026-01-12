using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip menuMusic;
    public AudioClip gameMusic;
    public AudioClip bossMusic;

    public AudioClip loseMusic;//ternyata sfx :V

    public AudioClip menuSelect;
    public AudioClip settingSelectSound;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip dead;
    public AudioClip hit;
    public AudioClip blockJatuh;
    public AudioClip blockJatuh2;
    public AudioClip blockMeledak;
    public AudioClip gameOver;
    public AudioClip bossKilled;
    public AudioClip attacked;
    public AudioClip lavaHiss;


    public static AudioManager instance;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string currentScene = scene.name;
        if(currentScene == "Main Menu_Final")
        {
            PlayMenuMusic();
        }
        else if (currentScene == "Phase1_origin" || currentScene == "Phase2_origin" || currentScene == "Survival")
        {
            PlayGameMusic();
        }else if(currentScene == "bossFight"){
            PlayBossMusic();
        }
        else{
            musicSource.Stop();
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

    public void PlayBossMusic(){
        musicSource.clip = bossMusic;
        musicSource.Play();
    }

    public void StopMusic(){
        musicSource.Stop();
    }

    // ternyata sfx :V
    // public void PlayLoseMusic(){
    //     musicSource.clip = loseMusic;
    //     musicSource.Play();
    // }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
