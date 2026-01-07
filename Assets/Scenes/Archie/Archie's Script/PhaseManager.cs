using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    public static PhaseManager instance;

    [Header("Scene-to-Load Names")]
    [SerializeField] string phase2;
    [SerializeField] string phaseBoss;

    [Space]

    [Header("Time Settings")]
    [SerializeField] float p1Time;
    [SerializeField] float p2Time;

    Phase currentPhase;
    bool p2Called = false;
    bool pBossCalled = false;

    public enum Phase
    {
        phase1,
        phase2,
        boss
    }

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentPhase = Phase.phase1;
    }

    void Update()
    {
        switch (currentPhase)
        {
            case Phase.phase1:
                p1Time -= Time.deltaTime;
                if (p1Time <= 0 && !p2Called)
                {
                    startPhase2();
                }
                break;
            case Phase.phase2:
                p2Time -= Time.deltaTime;
                    if (p2Time <= 0 && !pBossCalled)
                    {
                        startPhaseBoss();
                    }
                break;
            case Phase.boss:
                Debug.Log("Boss");
                break;
        }
    }

    void startPhase2()
    {
        p2Called = true;
        SceneManager.LoadSceneAsync(phase2);
        currentPhase = Phase.phase2;
    }

    void startPhaseBoss()
    {
        pBossCalled = true;
        SceneManager.LoadSceneAsync(phaseBoss);
        currentPhase = Phase.boss;
    }
}
