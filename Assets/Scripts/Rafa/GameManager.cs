using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   public static GameManager instance;
   public GameObject lose;
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

    
}
