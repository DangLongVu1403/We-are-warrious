using UnityEngine;
using System.Collections;

public class ArcherAlly : MonoBehaviour
{
    public float speed = 0.4f; // Tốc độ di chuyển
    public Animator animator; // Animator để điều khiển hoạt ảnh
    public GameObject arrowPrefab; // Prefab của mũi tên
    public HealthBar healthBar;
    public int id;
    public float damage = 20f;
    private SpawnSoldierController spawnController; 
    private bool isMoving = true; // Biến kiểm soát việc di chuyển
    private float shootCooldown = 0.5f; // Thời gian giữa các lần bắn
    private float lastShootTime; // Thời gian của lần bắn cuối cùng
    private DatabaseManager dbManager;
    private GameManager gameManager;
    private SpriteRenderer arrowRenderer; // Renderer của mũi tên

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        id = gameManager.getIdAlly();
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        dbManager = FindObjectOfType<DatabaseManager>();
    }

    void Start()
    {
        if (healthBar != null) // Kiểm tra xem healthBar có được gán không
        {
            DatabaseManager.SoldierAlly data = dbManager.GetSoldierAllyById(id);
            if (dbManager != null)
            {
                healthBar.SetMaxHealth(data.HealthAlly2);
                damage = data.DamageAlly2;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        healthBar.UpdateHealth(-amount);
    }

    void Update()
    {
        // Kiểm tra nếu đang di chuyển
        if (isMoving)
        {
            Move();
        }
        if (healthBar.GetCurrentHealth() <= 0)
        {
            if (spawnController != null) // Kiểm tra controller còn tồn tại
            {
                spawnController.RemoveThrowerAlly(gameObject); // Gọi hàm xóa lính khỏi danh sách
            }
        }
    }

    public void SetController(SpawnSoldierController controller) // Thiết lập controller
    {
        spawnController = controller;
    }

    void Move()
    {
        if (isMoving)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        // Tìm kiếm các đối tượng trong bán kính tấn công
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);

        GameObject nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        // Lặp qua các đối tượng tìm thấy để chọn mục tiêu gần nhất
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("EnemyHall"))
            {
                float distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);
                if (distanceToTarget < nearestDistance)
                {
                    nearestDistance = distanceToTarget;
                    nearestTarget = hit.gameObject;
                }
            }
        }

        // Nếu tìm thấy mục tiêu gần nhất
        if (nearestTarget != null)
        {
            isMoving = false; // Dừng di chuyển
            animator.SetInteger("Speed", 0); // Dừng hoạt ảnh di chuyển

            // Bắn cung nếu chưa bắn trong thời gian cooldown
            if (Time.time > lastShootTime + shootCooldown)
            {
                StartCoroutine(ShootArrow(nearestTarget)); // Bắt đầu bắn cung
            }
        }
        else
        {
            // Nếu không còn mục tiêu, tiếp tục di chuyển
            isMoving = true;
            animator.SetInteger("Speed", 1);
        }
    }

    private IEnumerator ShootArrow(GameObject target)
    {
        while (target != null) // Kiểm tra mục tiêu vẫn còn tồn tại
        {
            if (target == null)
            {
                isMoving = true;
                animator.SetInteger("Speed", 1);
                yield break;
            }
            yield return new WaitForSeconds(shootCooldown); 
            Shoot(target); // Bắn mũi tên vào mục tiêu
            lastShootTime = Time.time;
        }

        isMoving = true;
        animator.SetInteger("Speed", 1);
    }

    void Shoot(GameObject target)
{
    // Tạo mũi tên và bắn tới kẻ thù
    GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
    ArrowAlly arrowComponent = arrow.GetComponent<ArrowAlly>();
    
    if (arrowComponent != null) // Kiểm tra arrowComponent không phải null
    {
        arrowComponent.Initialize(target); // Gán mục tiêu cho mũi tên
    }
    else
    {
        Debug.LogError("ArrowAlly component not found on the arrow prefab!");
    }
}


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyHall"))
        {
            isMoving = true;
            animator.SetInteger("Speed", 1);
        }
    }
    public void OnThrowAnimationComplete()
    {
        // Hiện mũi tên khi animation ném hoàn tất
        arrowPrefab.SetActive(true);
        arrowRenderer.sortingOrder = 3; // Đưa mũi tên lên layer trên cùng
    }
}
