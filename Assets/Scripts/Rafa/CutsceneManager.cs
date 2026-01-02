using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class CutsceneManager : MonoBehaviour
{

    [SerializeField] public List<Sprite> images = new List<Sprite>();
    [SerializeField] Image displayImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       nextSlide();
    }


void nextSlide()
    {
        if (images.Count == 0)
        {
            //if 0 load next scene build index
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            //else show first image and remove it from the list
            displayImage.sprite = images[0];
            images.RemoveAt(0);
        }
    }
  

void Update()
    {
        // space or left mouse click to go to next slide
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.Mouse0))
        {
            nextSlide();
        }
    }
   
}