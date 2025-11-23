using System.Collections;
using UnityEngine;

public class BomberKill : MonoBehaviour
{
    [Header("Ticking Settings")]
    public int ticMany = 5; 
    public float timePerTic = 0.2f; 
    
    [Header("Explosion Settings")]
    public float initialDelay = 1.5f; 
    public float explosionDuration = 0.1f;
    public GameObject tetrisObject;
    
    [Header("References")]
    public GameObject[] tetrisBlock;
    private Color originalColor = Color.white; 
    
    private bool bombActivated = false;
    private bool playerOnBomb = false;
    private GameObject player;

    void Start()
    {
        if (tetrisBlock.Length > 0 && tetrisBlock[0] != null)
        {
            SpriteRenderer renderer = tetrisBlock[0].GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                originalColor = renderer.color;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !bombActivated)
        {
            bombActivated = true;
            StartCoroutine(InitialDelayAndTicking()); 
            playerOnBomb = true;
            player = collision.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player && bombActivated)
        {
            playerOnBomb = false;
        }
    }

    IEnumerator InitialDelayAndTicking()
    {
        yield return new WaitForSeconds(initialDelay);
        yield return StartCoroutine(BombTickingVisual()); 
            
        Explode();
    }

    IEnumerator BombTickingVisual()
    {
        for (int i = 0; i < ticMany; i++)
        {
            foreach (GameObject block in tetrisBlock)
            {
                SpriteRenderer temp = block.GetComponent<SpriteRenderer>();
                if (temp != null)
                {
                    temp.color = Color.red;
                }
            }

            yield return new WaitForSeconds(timePerTic / 2f); 
            
            foreach (GameObject block in tetrisBlock)
            {
                SpriteRenderer temp = block.GetComponent<SpriteRenderer>();
                if (temp != null)
                {
                    temp.color = originalColor;
                }
            }
            
            yield return new WaitForSeconds(timePerTic / 2f); 
        }
    
        foreach (GameObject block in tetrisBlock)
        {
            SpriteRenderer temp = block.GetComponent<SpriteRenderer>();
            if (temp != null)
            {
                temp.color = Color.red;
            }
        }
    }

    void Explode()
    {
        // Hancurkan block bom ini sendiri dan jika player diatas maka dia juga
        if (playerOnBomb && player != null)
        {
            Destroy(player);
        }
        Destroy(tetrisObject);
    }
}