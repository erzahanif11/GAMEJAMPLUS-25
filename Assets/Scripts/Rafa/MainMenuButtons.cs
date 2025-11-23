using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void MainScene()
    {
        SceneManager.LoadScene("Main Scene_Final");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void NextScene()
    {
        SceneManager.LoadScene("Main Scene_Final");
        AudioManager.instance.PlayGameMusic();
    }
}
