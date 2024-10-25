using UnityEngine;

public class RockEnemy : MonoBehaviour
{
    public float speed = 5f; // Tốc độ ném
    public float damage = 20f; // Sát thương
    private GameObject target; // Mục tiêu để ném
    private Vector3 startPoint; // Điểm bắt đầu ném
    private float flightDuration = 1.0f; // Thời gian bay
    private float elapsedTime = 0f; // Thời gian đã trôi qua
    private DatabaseManager databaseManager;
    private GameManager gameManager;
    private Vector3? lastTargetPosition = null; // Biến lưu vị trí cuối của target
    private int id;

    void Awake()
    {
        databaseManager = FindObjectOfType<DatabaseManager>();
        gameManager = FindObjectOfType<GameManager>();
        transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
        id = gameManager.getIdEnemy();
        DatabaseManager.SoldierAlly data = databaseManager.GetSoldierAllyById(id);
        damage = data.DamageAlly2;
    }

    // Khởi tạo mục tiêu và lưu điểm bắt đầu
    public void Initialize(GameObject target)
    {
        this.target = target;
        startPoint = transform.position; // Lưu điểm bắt đầu của viên đá
        Destroy(gameObject, 1.2f); // Hủy viên đá sau 3 giây nếu không va chạm
    }

    void Update()
    {
        if (target != null)
        {
            lastTargetPosition = target.transform.position;
        }

        elapsedTime += Time.deltaTime;

        // Tính toán tỷ lệ thời gian bay
        float t = elapsedTime / flightDuration;

        // Nếu target là null và đã có vị trí cuối cùng, dùng vị trí đó làm đích
        if (lastTargetPosition.HasValue)
        {
            Vector3 targetPosition = target != null ? target.transform.position : lastTargetPosition.Value;

            // Tính toán vị trí mới dựa trên quỹ đạo parabol
            Vector3 newPosition = Vector3.Lerp(startPoint, targetPosition, t);

            // Điều chỉnh trục Y theo hàm sin để tạo quỹ đạo cong
            float height = Mathf.Sin(t * Mathf.PI) * 0.5f; // Điều chỉnh hệ số để làm quỹ đạo thấp hơn
            newPosition.y += height;

            // Di chuyển viên đá theo vị trí đã tính toán
            transform.position = newPosition;

            // Kiểm tra nếu viên đá đã đến gần mục tiêu
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                if (target != null)
                {
                    // Nếu mục tiêu là kẻ địch, gây sát thương
                    if (target.CompareTag("Ally"))
                    {
                        AllyMovement ally = target.GetComponent<AllyMovement>();
                        if (ally != null)
                        {
                            ally.TakeDamage(damage);
                        }
                        Ally3Movement ally3 = target.GetComponent<Ally3Movement>();
                        if (ally3 != null)
                        {
                            ally3.TakeDamage(damage);
                        }
                        ArcherAlly ally21 = target.GetComponent<ArcherAlly>();
                        if (ally21 != null)
                        {
                            ally21.TakeDamage(damage);
                        }
                        ThrowerAlly ally2 = target.GetComponent<ThrowerAlly>();
                        if (ally2 != null)
                        {
                            ally2.TakeDamage(damage);
                        }
                    }
                    // Nếu mục tiêu là Hall của địch, gây sát thương cho Hall
                    else if (target.CompareTag("AllyHall"))
                    {
                        AllyHall allyHall = target.GetComponent<AllyHall>();
                        if (allyHall != null)
                        {
                            allyHall.TakeDamage(damage);
                        }
                    }
                }

                // Hủy viên đá sau khi gây sát thương hoặc khi đến vị trí cuối cùng
                Destroy(gameObject);
            }
        }
    }
}
