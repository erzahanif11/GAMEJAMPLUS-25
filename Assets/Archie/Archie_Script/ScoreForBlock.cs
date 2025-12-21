using UnityEngine;

public class ScoreForBlock : MonoBehaviour
{
    public int scorePerStack = 5;
    public bool hasStacked = false;


   

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Block") && !hasStacked && ScoreManager.Instance != null)
        {
            hasStacked = true;

            ScoreManager.Instance.AddScore(scorePerStack);

            Debug.Log("Tumpukan Blok Terdeteksi! Score +5.");
     
        }



    }
}


