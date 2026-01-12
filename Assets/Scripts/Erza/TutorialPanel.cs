using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    public  GameObject tutorialPanel;

    public GameObject[] tutorialPages;

    int currentIndex = 0;

    void Start()
    {
        tutorialPanel.SetActive(false);
    }

    void Update()
    {
        if (tutorialPanel.activeSelf && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            NextPage();
        }
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
        currentIndex = 0;
        tutorialPages[currentIndex].SetActive(true);
    }

    public void NextPage()
    {
        tutorialPages[currentIndex].SetActive(false);
        currentIndex++;
        if (currentIndex >= tutorialPages.Length)
        {
            CloseTutorial();
        }
    }

    void CloseTutorial()
    {
        foreach (var page in tutorialPages)
        {
            page.SetActive(true);
        }currentIndex = 0;
        tutorialPanel.SetActive(false);
    }
}
