using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    


    public float fillDuration = 1f; // Duration in seconds to fill the image

    public void Death()
    {
        Debug.Log("Player has died!");
        StartCoroutine(FillImage());
    }

    IEnumerator FillImage()
    {
        if (lose == null){
            yield break;
        }
        Image image = lose.GetComponent<Image>();
        
        if (image == null)
        {
            Debug.LogError("Image component not found on lose GameObject!");
            yield break;
        }

        float elapsedTime = 0f;
        image.fillAmount = 0f;

        while (image.fillAmount<1)
        {
            elapsedTime += Time.deltaTime;
            image.fillAmount = elapsedTime / fillDuration;
            yield return null;
        }
    }
}
