using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimerManager : MonoBehaviour
{
    TextMeshProUGUI timerTMP;
    float timer;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI bestTimeText;

    public static TimerManager instance;
    void Awake(){
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        timerTMP = GameObject.FindGameObjectWithTag("Timer").GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        RunTimer();
    }
    
    void RunTimer()
    {
        int menit = Mathf.FloorToInt(timer/60f);
        int detik = Mathf.FloorToInt(timer%60f);
        timerTMP.text = $"{menit:00}:{detik:00}";

        timer += Time.deltaTime;
    }

    public void SaveBestTime()
    {
        float bestTime = PlayerPrefs.GetFloat("BestTime", 0f);
        if (timer > bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", timer);
            bestTime = timer;
        }

        int menit = Mathf.FloorToInt(timer/60f);
        int detik = Mathf.FloorToInt(timer%60f);

        timeText.text = $"Your Time: {menit:00}:{detik:00}";

        int bmenit = Mathf.FloorToInt(bestTime/60f);
        int bdetik = Mathf.FloorToInt(bestTime%60f);
        
        bestTimeText.text = $"Best Time: {bmenit:00}:{bdetik:00}";
    }
}

