using UnityEngine;
using System.Collections;

public class ThrowerAlly : MonoBehaviour
{
    public float speed = 0.4f; // Tốc độ di chuyển
    public Animator animator; // Animator để điều khiển hoạt ảnh
    public GameObject rockPrefab; // Prefab của đá
    public HealthBar healthBar;
    public int id;
    public float damage = 20f;
    private SpawnSoldierController spawnController; 
    private bool isMoving = true; // Biến kiểm soát việc di chuyển
    private float throwCooldown = 0.5f; // Thời gian giữa các lần ném
    private float lastThrowTime; // Thời gian của lần ném cuối cùng
    private DatabaseManager dbManager;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        id = gameManager.getIdAlly();
        if(id == 1){
            transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        } else if(id == 2){
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        } else if(id == 3){
            transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        dbManager = FindObjectOfType<DatabaseManager>();
    }

    void Start(){
        if (healthBar != null) // Kiểm tra xem healthBar có được gán không
        {
            DatabaseManager.SoldierAlly data = dbManager.GetSoldierAllyById(id);
            if (dbManager != null){
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

    // void Move()
    // {
    //     // Di chuyển từ trái qua phải
    //     transform.Translate(Vector3.right * speed * Time.deltaTime);

    //     // Kiểm tra va chạm với các đối tượng có tag "Enemy" hoặc "EnemyHall"
    //     RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1.5f);
    //     if (hit.collider != null && (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("EnemyHall")))
    //     {
    //         // Dừng lại và set speed của animator = 0
    //         Debug.Log("dừng");
    //         isMoving = false;
    //         animator.SetInteger("Speed", 0);

    //         // Ném đá nếu chưa ném trong thời gian cooldown
    //         if (Time.time > lastThrowTime + throwCooldown)
    //         {
    //             StartCoroutine(ThrowRocks(hit.collider.gameObject)); // Bắt đầu ném đá liên tục
    //         }
    //     }
    //     else
    //     {
    //         // Nếu không còn kẻ thù, tiếp tục di chuyển
    //         isMoving = true;
    //         animator.SetInteger("Speed", 1);
    //     }
    // }
    void Move()
    {
        // Di chuyển từ trái qua phải
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

            // Ném đá nếu chưa ném trong thời gian cooldown
            if (Time.time > lastThrowTime + throwCooldown)
            {
                StartCoroutine(ThrowRocks(nearestTarget)); // Bắt đầu ném đá liên tục
            }
        }
        else
        {
            // Nếu không còn mục tiêu, tiếp tục di chuyển
            isMoving = true;
            animator.SetInteger("Speed", 1);
        }
    }



    private IEnumerator ThrowRocks(GameObject target)
    {
        yield return new WaitForSeconds(throwCooldown);
        while (target != null) // Kiểm tra mục tiêu vẫn còn tồn tại
        {
            // Nếu mục tiêu bị tiêu diệt hoặc không còn, thoát khỏi vòng lặp
            if (target == null)
            {
                isMoving = true; // Cho phép di chuyển lại
                animator.SetInteger("Speed", 1); // Set tốc độ của animator
                yield break; // Dừng Coroutine
            }

            ThrowRock(target); // Ném đá vào mục tiêu
            lastThrowTime = Time.time; // Cập nhật thời gian ném

            yield return new WaitForSeconds(throwCooldown); // Chờ trước khi ném lần tiếp theo
        }

        // Nếu mục tiêu đã biến mất, cho phép di chuyển tiếp
        isMoving = true;
        animator.SetInteger("Speed", 1);
    }


    void ThrowRock(GameObject target)
    {
        // Tạo đá và ném đến kẻ thù
        GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
        RockAlly rockComponent = rock.GetComponent<RockAlly>();
        rockComponent.Initialize(target); // Chuyển vào đối tượng kẻ thù
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Kiểm tra khi ra khỏi vùng va chạm
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyHall"))
        {
            // Tiếp tục di chuyển và set speed của animator về lại
            isMoving = true;
            animator.SetInteger("Speed", 1);
        }
    }
}
