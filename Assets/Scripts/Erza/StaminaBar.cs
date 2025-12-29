using UnityEngine;
using UnityEngine.UI;
public class StaminaBar : MonoBehaviour
{
    PlayerStats stats;
    public Slider staminaSlider;

    void Start()
    {
        stats = FindObjectOfType<PlayerStats>();
        staminaSlider.maxValue = 12f;
    }

    void Update()
    {
        staminaSlider.value = stats.stamina;
    }
}
