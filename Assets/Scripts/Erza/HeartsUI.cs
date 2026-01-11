using UnityEngine;
using UnityEngine.UI;

public class HeartsUI : MonoBehaviour
{
    public Image[] hearts;
    PlayerStats stats;

    void Start(){
        stats = FindAnyObjectByType<PlayerStats>();
        UpdateHeartsUI();
    }

    void Update(){
        UpdateHeartsUI();
    }

    void UpdateHeartsUI(){
        for (int i = 0; i < hearts.Length; i++){
            if (i < stats.lives){
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }
    }
}
