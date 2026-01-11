using UnityEngine;
using System.Collections; // Wajib ada untuk IEnumerator
using Unity.Cinemachine;

public class BulletScript : MonoBehaviour
{
    // Attach this to the player's hitbox/collider.

    [Header("Detection")]
    public string bulletTag = "hellbullet";
    public bool destroyBulletOnHit = true;

    [Header("Immunity Settings")]
    public float immunityDuration = 1.0f; // Durasi kebal (1 detik)
    public float blinkInterval = 0.1f;    // Seberapa cepat kedipnya

    private bool isImmune = false;        // Status apakah sedang kebal
    private PlayerStats playerStats;
    private SpriteRenderer spriteRenderer;
    PlayerMovement playerMovement;
    CinemachineImpulseSource impulseSource;



    void Awake()
    {
                impulseSource = GetComponent<CinemachineImpulseSource>();

    }
    void Start()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
        playerMovement = GetComponent<PlayerMovement>();
        // Mengambil komponen SpriteRenderer di object ini (atau di parent jika script di child)
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) 
        {
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Cek Tag
        if (!other.CompareTag(bulletTag)) return;

        // 2. Cek apakah sedang Immune? Jika ya, abaikan peluru.
        if (isImmune || playerMovement.isDashing) return;


        // 3. Logika kena damage
        if (playerStats != null)
        {
            playerStats.TakeDamage();
            CameraShakeManager.instance.CameraShake(impulseSource);

            AudioManager.instance.PlaySFX(AudioManager.instance.attacked);
            if (playerStats.lives <= 0)
            {
                GameManager.instance.Death();
            }
        }
        

        if (destroyBulletOnHit)
        {
            Destroy(other.gameObject);
        }

        // 4. Mulai proses Imunitas & Blinking
        StartCoroutine(ActivateImmunity());
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // 1. Cek Tag
        if (!other.gameObject.CompareTag(bulletTag)) return;

        // 2. Cek apakah sedang Immune? Jika ya, abaikan peluru.
        if (isImmune || playerMovement.isDashing) return;


        // 3. Logika kena damage
        if (playerStats != null)
        {
            playerStats.TakeDamage();
            AudioManager.instance.PlaySFX(AudioManager.instance.attacked);
            if (playerStats.lives <= 0)
            {
                GameManager.instance.Death();
            }
        }
        

        if (destroyBulletOnHit)
        {
            Destroy(other.gameObject);
        }

        // 4. Mulai proses Imunitas & Blinking
        StartCoroutine(ActivateImmunity());
    }

    // Coroutine untuk menangani durasi dan animasi kedip
    IEnumerator ActivateImmunity()
    {
        isImmune = true; // Aktifkan status kebal
        float timer = 0f;

        while (timer < immunityDuration)
        {
            // Toggle visibility sprite (Hidup/Mati) untuk efek kedip
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            // Tunggu sesuai interval kedip
            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // Selesai Imunitas
        isImmune = false;
        
        // Pastikan sprite terlihat kembali di akhir
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }
}