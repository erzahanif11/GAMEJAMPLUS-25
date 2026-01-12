using UnityEngine;
using System.Collections;

public class LavaEater : MonoBehaviour
{
    public float sinkSpeed = 5e-16f;
    PlayerStats playerStats;
    bool isImmune;
    float immunityDuration = 1.0f; 
    float blinkInterval = 0.1f;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
        spriteRenderer = GameObject.FindGameObjectByTag("Player").GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //get parent object
        GameObject parentObject = other.transform.parent != null ? other.transform.parent.gameObject : other.gameObject;    
        
        if (parentObject.CompareTag("Block"))
        {
            Rigidbody2D rb = parentObject.GetComponent<Rigidbody2D>();

            if (rb .bodyType !=RigidbodyType2D.Kinematic )
            {
                //+5 poin
               rb.bodyType = RigidbodyType2D.Kinematic;
               rb.linearVelocity = new Vector2(0, 0);
            }

           parentObject.transform.position -= new Vector3(0, sinkSpeed * Time.deltaTime, 0);
        }
        else  if (other.CompareTag("Player") && GameManager.instance != null)
        {
            playerStats.TakeDamage();
            if (playerStats.lives <= 0)
            {
                GameManager.instance.Death();

            }
            GameManager.instance.RespawnPlayer();
            StartCoroutine(ActivateImmunity());
        }

    }

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
