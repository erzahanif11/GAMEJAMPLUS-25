using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class BossAI : MonoBehaviour
{
    private HellShooter shooter;
    [SerializeField] private float patternLoopDuration = 180; // 3 menit (180 detik)

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] private Animator bossAnimator;
    void Start()
    {
        shooter = GetComponent<HellShooter>();

        // Initialize displayed time to the full duration
        if (timeText != null)
        {
            int totalSeconds = Mathf.CeilToInt(patternLoopDuration);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        StartCoroutine(BossPattern(patternLoopDuration));
    }

    IEnumerator BossPattern(float duration)
    {

        float endTime = Time.time + duration;
        StartCoroutine(UpdateRemainingTime(endTime));

       yield return new WaitForSeconds(3f);



        // Start UI updater coroutine that shows remaining time as MM:SS

        while (Time.time < endTime)
        {

            int random = UnityEngine.Random.Range(0,4);
            bossAnimator.SetTrigger("Attack");

            if (random == 0) 
                shooter.StartPhase("Tracking", 5f); 
            else if (random == 1) 
                shooter.StartPhase("Nova", 5f); 
            else if (random == 2) 
                shooter.StartPhase("Spiral", 5f);
            else if (random == 3) 
                shooter.StartPhase("TripleTracking", 5f);



            yield return new WaitForSeconds(5.2f); // Tunggu sampai fase selesai + jeda dikit


            bossAnimator.SetTrigger("Idle");
            yield return new WaitForSeconds(2f);


          
        }

        Debug.Log("Selesai boss pattern setelah " + duration + " detik.");
        if (timeText != null) timeText.text = "00:00";


        bossAnimator.SetTrigger("Death");
        AudioManager.instance.PlaySFX(AudioManager.instance.bossKilled);
            yield return new WaitForSeconds(5f);

        // next scene


    nextScene.OnClick();
        
    }

    // Updates the time text to show remaining time in MM:SS format
    IEnumerator UpdateRemainingTime(float endTime)
    {
        while (Time.time < endTime)
        {
            float remaining = endTime - Time.time;
            int remainingSeconds = Mathf.CeilToInt(remaining);
            int minutes = remainingSeconds / 60;
            int seconds = remainingSeconds % 60;
            if (timeText != null)
                timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
        }
        if (timeText != null)
            timeText.text = "00:00";
    }



}