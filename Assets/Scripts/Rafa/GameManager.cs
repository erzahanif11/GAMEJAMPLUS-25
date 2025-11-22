using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public static GameManager instance;
   public GameObject lose;
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
    


    public void Death()
    {
        lose.SetActive(true);
        Debug.Log("Player has died!");
    }
}
