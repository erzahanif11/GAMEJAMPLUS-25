using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Onclick()
    {
        Time.timeScale=1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayGameMusic();
        }
    }
}
