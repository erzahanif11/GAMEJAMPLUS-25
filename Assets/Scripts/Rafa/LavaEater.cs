using UnityEngine;

public class LavaEater : MonoBehaviour
{
    public float sinkSpeed = 5e-16f;
    PlayerStats playerStats;

    void Awake()
    {
        playerStats = FindAnyObjectByType<PlayerStats>();
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
        }


    }
}
