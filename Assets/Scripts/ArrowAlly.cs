using UnityEngine;

public class ArrowAlly : MonoBehaviour
{
    public float speed = 5f; // Tốc độ bay của mũi tên
    public float damage = 20f; // Sát thương
    private GameObject target; // Mục tiêu bắn
    private DatabaseManager databaseManager;
    private GameManager gameManager;
    private int id;

    void Awake()
    {
        databaseManager = FindObjectOfType<DatabaseManager>();
        gameManager = FindObjectOfType<GameManager>();
        
        // Thiết lập kích thước của mũi tên
        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        id = gameManager.getIdAlly();
    }
    void start(){
        DatabaseManager.SoldierAlly data = databaseManager.GetSoldierAllyById(id);
        damage = data.DamageAlly2;
    }

    // Khởi tạo mục tiêu
    public void Initialize(GameObject target)
    {
        this.target = target;
        Destroy(gameObject, 1f); // Hủy mũi tên sau 3 giây nếu không va chạm
    }

    void Update()
    {
        if (target != null)
        {
            // Di chuyển mũi tên thẳng tới mục tiêu
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Xoay mũi tên theo hướng di chuyển
            if (direction != Vector3.zero) // Kiểm tra xem hướng không phải là vector không
            {
                // Tính góc quay theo hướng di chuyển
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }

            // Kiểm tra nếu mũi tên đã đến gần mục tiêu
            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                // Nếu mục tiêu là kẻ địch, gây sát thương
                if (target.CompareTag("Enemy"))
                {
                    EnemyMovement enemy = target.GetComponent<EnemyMovement>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                    ThrowerEnemy throwerEnemy = target.GetComponent<ThrowerEnemy>();
                    if (throwerEnemy != null)
                    {
                        throwerEnemy.TakeDamage(damage);
                    }
                    Enemy3Movement enemy3 = target.GetComponent<Enemy3Movement>();
                    if (enemy3 != null)
                    {
                        enemy3.TakeDamage(damage);
                    }
                }
                // Nếu mục tiêu là Hall của địch, gây sát thương cho Hall
                else if (target.CompareTag("EnemyHall"))
                {
                    EnemyHall enemyHall = target.GetComponent<EnemyHall>();
                    if (enemyHall != null)
                    {
                        enemyHall.TakeDamage(damage);
                        enemyHall.DropGold();
                    }
                }

                // Hủy mũi tên sau khi gây sát thương
                Destroy(gameObject);
            }
        }
    }
}
