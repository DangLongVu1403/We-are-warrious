using UnityEngine;
using System.Collections;

public class ArcherEnemy : MonoBehaviour
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

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        id = gameManager.getIdEnemy(); // Sửa id cho enemy
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        dbManager = FindObjectOfType<DatabaseManager>();
    }

    void Start()
    {
        if (healthBar != null)
        {
            DatabaseManager.SoldierEnemy data = dbManager.GetSoldierEnemyById(id);
            if (dbManager != null)
            {
                healthBar.SetMaxHealth(data.HealthEnemy2);
                damage = data.DamageEnemy2;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        healthBar.UpdateHealth(-amount);
    }

    void Update()
    {
        if (isMoving)
        {
            Move();
        }

        if (healthBar.GetCurrentHealth() <= 0)
        {
            if (spawnController != null)
            {
                spawnController.RemoveThrowerEnemy(gameObject);
            }
        }
    }

    public void SetController(SpawnSoldierController controller)
    {
        spawnController = controller;
    }

    void Move()
    {
        if (isMoving)
        {
            // Di chuyển từ phải sang trái
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

        // Tìm kiếm các đối tượng ally trong bán kính tấn công
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1.5f);

        GameObject nearestTarget = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            // Kiểm tra nếu đối tượng là ally hoặc AllyHall
            if (hit.CompareTag("Ally") || hit.CompareTag("AllyHall"))
            {
                float distanceToTarget = Vector3.Distance(transform.position, hit.transform.position);
                if (distanceToTarget < nearestDistance)
                {
                    nearestDistance = distanceToTarget;
                    nearestTarget = hit.gameObject;
                }
            }
        }

        if (nearestTarget != null)
        {
            isMoving = false;
            animator.SetInteger("Speed", 0); // Dừng di chuyển

            if (Time.time > lastShootTime + shootCooldown)
            {
                StartCoroutine(ShootArrow(nearestTarget));
            }
        }
        else
        {
            isMoving = true;
            animator.SetInteger("Speed", 1);
        }
    }

    private IEnumerator ShootArrow(GameObject target)
    {
        while (target != null)
        {
            yield return new WaitForSeconds(shootCooldown);
            Shoot(target);
            lastShootTime = Time.time;
        }

        isMoving = true;
        animator.SetInteger("Speed", 1);
    }

    void Shoot(GameObject target)
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        ArrowEnemy arrowComponent = arrow.GetComponent<ArrowEnemy>();

        if (arrowComponent != null)
        {
            arrowComponent.Initialize(target);
        }
    }
}
