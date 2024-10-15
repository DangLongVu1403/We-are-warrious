using UnityEngine;

public class RockAlly : MonoBehaviour
{
    public float speed = 5f; // Tốc độ ném
    public float damage = 20f; // Sát thương
    private GameObject target; // Mục tiêu để ném
    private Vector3 startPoint; // Điểm bắt đầu ném
    private float flightDuration = 1.0f; // Thời gian bay
    private float elapsedTime = 0f; // Thời gian đã trôi qua
    private DatabaseManager databaseManager;
    private GameManager gameManager;
    private int id;
    void Awake()
    {
        databaseManager = FindObjectOfType<DatabaseManager>();
        gameManager = FindObjectOfType<GameManager>();
        
        // Thiết lập kích thước viên đá
        transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
        id = gameManager.getIdAlly();
        DatabaseManager.SoldierAlly data = databaseManager.GetSoldierAllyById(id);
        damage = data.DamageAlly2;
    }

    // Khởi tạo mục tiêu và lưu điểm bắt đầu
    public void Initialize(GameObject target)
    {
        this.target = target;
        startPoint = transform.position; // Lưu điểm bắt đầu của viên đá
        Destroy(gameObject, 1f); // Hủy viên đá sau 3 giây nếu không va chạm
    }

    void Update()
    {
        if (target != null)
        {
            elapsedTime += Time.deltaTime;

            // Tính toán tỷ lệ thời gian bay
            float t = elapsedTime / flightDuration;

            // Tính toán vị trí mới dựa trên quỹ đạo parabol
            Vector3 newPosition = Vector3.Lerp(startPoint, target.transform.position, t);
            
            // Điều chỉnh trục Y theo hàm sin để tạo quỹ đạo cong
            float height = Mathf.Sin(t * Mathf.PI) * 0.5f; // Điều chỉnh hệ số để làm quỹ đạo thấp hơn
            newPosition.y += height;

            // Di chuyển viên đá theo vị trí đã tính toán
            transform.position = newPosition;

            // Kiểm tra nếu viên đá đã đến gần mục tiêu
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
                
                // Hủy viên đá sau khi gây sát thương
                Destroy(gameObject);
            }
        }
    }
}
