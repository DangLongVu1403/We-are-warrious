using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f; // Máu tối đa
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth; // Khởi tạo máu hiện tại
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Giảm máu
        if (currentHealth <= 0)
        {
            Die(); // Gọi hàm chết nếu máu <= 0
        }
    }

    private void Die()
    {
        Destroy(gameObject); // Xóa đối tượng khi chết
    }
}
