using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void MainScene()
    {
        SceneManager.LoadScene("Main Scene");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        AudioManager.instance.PlayGameMusic();
    }
}
