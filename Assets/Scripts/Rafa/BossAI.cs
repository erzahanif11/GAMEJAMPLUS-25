using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    private HellShooter shooter;
    [SerializeField] private float patternLoopDuration = 180f; // 3 menit (180 detik)

    void Start()
    {
        shooter = GetComponent<HellShooter>();
        StartCoroutine(BossPattern(patternLoopDuration));
    }

    IEnumerator BossPattern(float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            // Urutan bisa diatur semau Anda
            Debug.Log("Mulai Phase Tracking");
            shooter.StartPhase("Tracking", 5f); // Lakukan tracking selama 5 detik
            yield return new WaitForSeconds(6f); // Tunggu sampai fase selesai + jeda dikit


            // Debug.Log("Mulai Phase Triple Tracking");
            // shooter.StartPhase("TripleTracking", 5f);
            // yield return new WaitForSeconds(6f);

            
            Debug.Log("Mulai Phase Spiral");
            shooter.StartPhase("Spiral", 4f);
            yield return new WaitForSeconds(5f);

            Debug.Log("Mulai Phase Nova");
            shooter.StartPhase("Nova", 3f);
            yield return new WaitForSeconds(4f);
        }

        Debug.Log("Selesai boss pattern setelah " + duration + " detik.");
        // Opsional: trigger perilaku selanjutnya atau restart di sini
    }
}