using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider; // Tham chiếu đến Slider UI

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void UpdateHealth(float amount)
    {
        slider.value = Mathf.Clamp(slider.value + amount, 0, slider.maxValue); // Cập nhật và giới hạn giá trị thanh máu
    }

    public float GetCurrentHealth()
    {
        return slider.value; // Trả về giá trị máu hiện tại
    }
}
