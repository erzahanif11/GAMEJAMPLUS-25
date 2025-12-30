using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int lives = 9;
    int maxLives = 9;
    public float stamina = 12f;
    readonly float staminaRegenerate = 3f;
    public bool CanRegenerate = true;

    void Awake(){
        stamina = 12f;
        lives = maxLives;
    }

    void Update(){
        if (CanRegenerate)
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

    public bool UseStamina(float amount){
        if (stamina >= amount){
            stamina -= amount;
            return true;
        }
        return false;
    }

    public bool HasStamina(float amount)
    {
        return stamina >= amount;
    }
    public void TakeDamage(){
        lives -= 1;
    }


}
