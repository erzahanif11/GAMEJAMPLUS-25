using UnityEngine;

public class ScoreForBlock : MonoBehaviour
{
    public int ScorePerStack = 5;
    public bool hasStacked = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Block") && !hasStacked)
        {
            hasStacked = true;

        }
    }
}
