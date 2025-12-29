using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int lives = 9;
    int maxLives = 9;
    public float stamina = 12f;
    readonly float staminaRegenerate = 3f;

    void Awake(){
        stamina = 12f;
        lives = maxLives;
    }

    void Update(){
        RegenerateStamina();
    }

    void RegenerateStamina(){
        if (stamina < 12){
            stamina += staminaRegenerate * Time.deltaTime;
            if (stamina > 12){
                stamina = 12;
            }
        }
    }

    public bool UseStamina(){
        if (stamina >= 4f){
            stamina -= 4f;
            return true;
        }
        return false;
    }

    public void TakeDamage(){
        lives -= 1;
    }


}
