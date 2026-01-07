using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    public void Settings()
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
        Debug.Log("Settings Button Pressed");
    }
}
