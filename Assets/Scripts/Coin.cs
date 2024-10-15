using UnityEngine;

public class Coin : MonoBehaviour
{
    public Transform target;  // Vị trí tổng vàng
    public float moveSpeed = 5f;  // Tốc độ di chuyển của coin

    private bool isMoving = false;

    // Gọi hàm này để kích hoạt di chuyển
    void Awake(){
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    public void Trigger()
    {
        isMoving = true;
    }

    void Update()
    {
        if (isMoving && target != null)
        {
            // Di chuyển coin đến vị trí tổng vàng
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            // Kiểm tra nếu coin đã tới vị trí tổng vàng
            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                // Hủy coin khi tới đích (hoặc có thể tăng vàng rồi hủy)
                Destroy(gameObject);
            }
        }
    }
}
