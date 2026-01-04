using UnityEngine;
using System.Collections;



public class HellShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform player;
    public float bulletSpeed = 10f; // Kecepatan konstan peluru

    private Coroutine currentPhaseCoroutine;

    public void StartPhase(string phaseName, float duration)
    {
        if (currentPhaseCoroutine != null) StopCoroutine(currentPhaseCoroutine);
        
        if (phaseName == "Tracking") currentPhaseCoroutine = StartCoroutine(PhaseTracking(duration));
        else if (phaseName == "Nova") currentPhaseCoroutine = StartCoroutine(PhaseNova(duration));
        else if (phaseName == "Spiral") currentPhaseCoroutine = StartCoroutine(PhaseSpiral(duration));
    }

    // Fungsi Utama: Spawn peluru dengan arah tertentu
    void SpawnBullet(float angle)
    {
        // 1. Instantiate
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        
        // 2. Ambil Rigidbody2D
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            // 3. Konversi angle ke Vector2 arah
            // Kita gunakan transform.up dari rotasi peluru yang baru dibuat
            rb.linearVelocity = bullet.transform.up * bulletSpeed;
        }
    }

    // --- PHASE LOGIC ---

    IEnumerator PhaseTracking(float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            if (player != null)
            {
                // Ambil angle ke player saat ini
                Vector2 dir = player.position - transform.position;
                float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
                

                SpawnBullet(targetAngle);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator PhaseNova(float duration)
    {
        float endTime = Time.time + duration;
        int count = 12;
        while (Time.time < endTime)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnBullet(i * (360f / count));
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator PhaseSpiral(float duration)
    {
        float endTime = Time.time + duration;
        float currentAngle = 0f;
        while (Time.time < endTime)
        {
            SpawnBullet(currentAngle);
            currentAngle += 15f; 
            yield return new WaitForSeconds(0.05f);
        }
    }
}
