using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;

    [Header("Time Settings")]
    [SerializeField] float phase1Time = 60f;
    [SerializeField] float phase2Time = 180f;
    [SerializeField] float bossTime = 180f;

    float timer;
    TextMeshProUGUI timerTMP;

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int index = scene.buildIndex;

        // Reset timer tiap masuk phase
        if (index == 1) timer = phase1Time;
        else if (index == 3) timer = phase2Time;
        else if (index  == 5) timer = bossTime;
    }

    void Update()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        switch (index)
        {
            case 1: // Cutscene 1
                if (PrologueManager1.instance.IsFinished())
                    LoadNextScene();
                break;

            case 2: // Phase 1
                timerTMP = GameObject.FindGameObjectWithTag("Timer").GetComponent<TextMeshProUGUI>();
                RunTimer();
                break;

            case 3: // Cutscene 2
                if (PrologueManager2.instance.IsFinished())
                    LoadNextScene();
                break;

            case 4: // Phase 2
                timerTMP = GameObject.FindGameObjectWithTag("Timer").GetComponent<TextMeshProUGUI>();
                RunTimer();
                break;

            case 5: // Cutscene 3
                if (PrologueManager3.instance.IsFinished())
                    LoadNextScene();
                break;

            case 6: // Boss
                // Boss logic handled di BossManager
                RunTimer();
                break;
        }
    }

    void RunTimer()
    {
        if(timer > 0)
        {
            int menit = Mathf.FloorToInt(timer/60f);
            int detik = Mathf.FloorToInt(timer%60f);
            timerTMP.text = $"{menit:00}:{detik:00}";
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadSceneAsync(nextIndex);
    }
}
