using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public static GameManager instance;
   public GameObject lose;
   public GameObject player;
   public Transform checkpoint;
   public bool immune=false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    

    public float fillDuration = 1f; // Duration in seconds to fill the image

    public void Death()
    {
        if (immune)
        return;
        AudioManager.instance.PlaySFX(AudioManager.instance.dead);
        Debug.Log("Player has died!");
        lose.SetActive(true);
        Time.timeScale=0;
        AudioManager.instance.PlaySFX(AudioManager.instance.gameOver);
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Collider2D col = player.GetComponent<Collider2D>();
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        rb.simulated = false;
        col.enabled = false;
        sr.enabled = false;
        yield return new WaitForSeconds(1f); // Wait for 1 second before respawning
        player.transform.position = checkpoint.position;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.simulated = true;
        col.enabled = true;
        sr.enabled = true;
    }
}