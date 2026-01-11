using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
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
        // Reload the current scene by its build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void LoadSurvival(){
        SceneManager.LoadScene("Phase2_origin");
    }
}
