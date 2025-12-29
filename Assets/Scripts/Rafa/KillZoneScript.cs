using UnityEngine;

public class KillZoneScript : MonoBehaviour
{
    public Collider2D killZoneCollider;
    PlayerStats playerStats;

    void Awake()
    {
        if (killZoneCollider == null)
        killZoneCollider = GetComponent<Collider2D>();
        playerStats = FindAnyObjectByType<PlayerStats>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if block speed y is < -0.5 kill player
        if (other.CompareTag("Player")&& GameManager.instance != null && gameObject.GetComponentInParent<Rigidbody2D>().linearVelocityY < -0.5f)
        {
            playerStats.TakeDamage();
            if (playerStats.lives <= 0)
            {
                GameManager.instance.Death();
            }
            GameManager.instance.RespawnPlayer();
        }
        
        
        
    }

}
