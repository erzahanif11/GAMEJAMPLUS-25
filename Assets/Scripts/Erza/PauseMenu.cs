using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    
    void Update(){
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TooglePauseMenu();
        }
    }
    public void TooglePauseMenu()
    {
        if (pauseMenuPanel.activeSelf)
        {
            pauseMenuPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            pauseMenuPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
