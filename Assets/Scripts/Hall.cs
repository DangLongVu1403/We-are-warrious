using UnityEngine;
using UnityEngine.UI;

public class Hall : MonoBehaviour
{
    public Slider slider; // Tham chiếu đến Slider UI cho thanh máu

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void TakeDamage(float amount)
    {
        slider.value = Mathf.Clamp(slider.value - amount, 0, slider.maxValue);
    }

    public float GetCurrentHealth()
    {
        return slider.value;
    }
}
