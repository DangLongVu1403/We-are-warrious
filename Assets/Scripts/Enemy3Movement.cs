using UnityEngine;
using System.Collections;
using UnityEngine.Localization;
using TMPro;

public class Enemy3Movement : MonoBehaviour
{
    public float speed = 0.4f;
    public float damage = 20f;
    public int id;
    public GameObject goldPrefab;
    public LocalizedString goldTextLocalized;
    public TextMeshProUGUI goldText;
    public Transform goldTarget;
    public int goldAmount = 4;
    public HealthBar healthBar;
    private Animator animator;
    private bool isColliding = false; // Biến để theo dõi va chạm
    private Coroutine damageCoroutine; // Coroutine để trừ máu liên tục
    private SpawnSoldierController spawnController; // Tham chiếu đến controller quản lý spawn
    private float separationDistance = 0.2f; // Khoảng cách tách nhau giữa các lính
    private DatabaseManager dbManager;
    private GameManager gameManager;
    private AllyMovement allyMovement;

    void Awake(){
        dbManager = FindObjectOfType<DatabaseManager>();
        gameManager = FindObjectOfType<GameManager>();
        id = gameManager.getIdEnemy();
    }
    void Start()
    {
        if(id == 1){
            transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
        } else if(id == 2){
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        } else if(id == 3){
            transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        }
        animator = GetComponent<Animator>();

    // Lấy dữ liệu từ database khi bắt đầu nếu có
        if (dbManager != null)
        {
            DatabaseManager.SoldierEnemy data = dbManager.GetSoldierEnemyById(id); 
            if (data != null)
            {
                healthBar.SetMaxHealth(data.HealthEnemy3);
                damage = data.DamageEnemy3;
            }
        }
    }

    public void SetController(SpawnSoldierController controller) // Thiết lập controller
    {
        spawnController = controller;
    }

    void UpdateGoldText(string localizedGoldText)
    {
        // Lấy thông tin vàng hiện tại
        DatabaseManager.GameData gameData = dbManager.GetGold();
        
        // Cập nhật văn bản dựa trên ngôn ngữ
        goldText.text = localizedGoldText + gameData.GoldRoundCurrent;
    }
    void Update()
    {
        // Kiểm tra xem lính có quá gần lính khác không và tách ra
        SeparateFromEnemies();
        DatabaseManager.GameData gameData = dbManager.GetGold();
        UpdateGoldText(goldTextLocalized.GetLocalizedString());

        if (!isColliding) // Nếu không va chạm
        {
            animator.SetInteger("Speed", 1); // Di chuyển
            transform.Translate(Vector3.left * speed * Time.deltaTime); // Di chuyển ngược lại
        }
        else
        {
            animator.SetInteger("Speed", 0); // Dừng lại
        }

        // Giới hạn vị trí Y trong khoảng -0.3f đến 0.3f
        float clampedY = Mathf.Clamp(transform.position.y, -0.1f, 0.5f);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);

        // Kiểm tra máu của lính
        if (healthBar.GetCurrentHealth() <= 0)
        {
            DropGold();
            if (spawnController != null) // Kiểm tra controller còn tồn tại
            {
                spawnController.RemoveEnemySoldier(gameObject); // Gọi hàm xóa lính khỏi danh sách
            }
        }
    }

    public void TakeDamage(float amount)
    {
        healthBar.UpdateHealth(-amount);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ally")) // Kiểm tra va chạm với đồng minh
        {
            isColliding = true;
            animator.SetInteger("Speed", 0);

            if (damageCoroutine == null) 
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(other.gameObject));
            }
        }
        else if (other.gameObject.CompareTag("AllyHall")) // Kiểm tra va chạm với Hall
        {
            isColliding = true;
            animator.SetInteger("Speed", 0);

            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(DealDamageOverTimeHall(other.gameObject)); // Gọi Coroutine gây sát thương cho Hall
            }
        }
    }

    IEnumerator DealDamageOverTimeHall(GameObject hallObj)
    {
        Hall hall = hallObj.GetComponent<Hall>(); // Giả sử bạn có một lớp Hall để quản lý thanh máu lâu đài
        while (hall != null && isColliding)
        {
            yield return new WaitForSeconds(0.5f);
            hall.TakeDamage(damage); // Gây sát thương cho Hall
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ally") || other.gameObject.CompareTag("AllyHall"))
        {
            isColliding = false; // Không còn va chạm
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine); // Dừng Coroutine khi không còn va chạm
                damageCoroutine = null;
            }
        }
    }

    IEnumerator DealDamageOverTime(GameObject allyObj)
    {
        AllyMovement ally = allyObj.GetComponent<AllyMovement>();
        while (ally != null && isColliding)
        {
            yield return new WaitForSeconds(0.5f);
            if (ally != null) // Kiểm tra ally có còn tồn tại không
            {
                ally.TakeDamage(damage);
            }
        }
        Ally3Movement ally3 = allyObj.GetComponent<Ally3Movement>();
        while (ally3 != null && isColliding)
        {
            yield return new WaitForSeconds(0.5f);
            if (ally3 != null) // Kiểm tra ally có còn tồn tại không
            {
                ally3.TakeDamage(damage);
            }
        }
        ArcherAlly ally21 = allyObj.GetComponent<ArcherAlly>();
        while (ally21 != null && isColliding)
        {
            yield return new WaitForSeconds(0.5f);
            if (ally21 != null) // Kiểm tra ally có còn tồn tại không
            {
                ally21.TakeDamage(damage);
            }
        }
        ThrowerAlly ally2 = allyObj.GetComponent<ThrowerAlly>();
        while (ally2 != null && isColliding)
        {
            yield return new WaitForSeconds(0.5f);
            if (ally2 != null) // Kiểm tra ally có còn tồn tại không
            {
                ally2.TakeDamage(damage);
            }
        }
    }

    void SeparateFromEnemies()
    {
        EnemyMovement[] enemies = FindObjectsOfType<EnemyMovement>(); // Lấy tất cả lính địch

        foreach (EnemyMovement enemy in enemies)
        {
            if (enemy != this) // Không tự kiểm tra bản thân
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < separationDistance) // Nếu khoảng cách nhỏ hơn khoảng tách
                {
                    // Tính toán khoảng cách cần tách
                    float offsetY = transform.position.y > enemy.transform.position.y ? -separationDistance : separationDistance;
                    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
                    
                    // Giới hạn vị trí Y trong khoảng -0.3f đến 0.3f
                    newPosition.y = Mathf.Clamp(newPosition.y, -0.1f, 0.5f);

                    transform.position = newPosition; // Cập nhật vị trí
                }
            }
        }
    }
    void DropGold()
    {
        if (goldPrefab != null)
        {
            // Tạo coin tại vị trí của lính
            GameObject goldCoin = Instantiate(goldPrefab, transform.position, Quaternion.identity);

            // Bắt đầu quá trình di chuyển coin
            Coin coinScript = goldCoin.GetComponent<Coin>();
            if (coinScript != null)
            {
                coinScript.target = goldTarget;  // Gán vị trí tổng vàng
                coinScript.Trigger();  // Kích hoạt coin bay lên
                DatabaseManager.GameData gameData = dbManager.GetGold();
                int currentGold = gameData.GoldRoundCurrent + goldAmount;
                dbManager.UpdateGoldRoundCurrent(currentGold);
                int newGoldUpdate = gameData.NewGoldUpdate + goldAmount;
                dbManager.UpdateNewGoldUpdate(newGoldUpdate);

            }
        }
    }
}
