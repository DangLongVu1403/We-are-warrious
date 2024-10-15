using UnityEngine;
using System.Collections;
using UnityEngine.Localization;
using TMPro;

public class ThrowerEnemy : MonoBehaviour
{
    public float speed = 0.4f; // Tốc độ di chuyển
    public Animator animator; // Animator để điều khiển hoạt ảnh
    public GameObject rockPrefab; // Prefab của đá
    public float damage = 20f;
    public int goldAmount = 3;
    public Transform goldTarget;
    public LocalizedString goldTextLocalized;
    public TextMeshProUGUI goldText;
    public GameObject goldPrefab;
    public int id;
    private DatabaseManager dbManager;
    public HealthBar healthBar;
    private SpawnSoldierController spawnController; 
    private bool isMoving = true; // Biến kiểm soát việc di chuyển
    private float throwCooldown = 0.5f; // Thời gian giữa các lần ném
    private float lastThrowTime; // Thời gian của lần ném cuối cùng
    public float stopDistance = 2f; // Khoảng cách dừng từ đồng minh
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        id = gameManager.getIdEnemy();
        transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
        dbManager = FindObjectOfType<DatabaseManager>();
    }
    void Start(){
        if (healthBar != null) // Kiểm tra xem healthBar có được gán không
        {
            DatabaseManager.SoldierEnemy data = dbManager.GetSoldierEnemyById(id);
            if (dbManager != null){
                healthBar.SetMaxHealth(data.HealthEnemy2);
                damage = data.DamageEnemy2;
            }
        }
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
        DatabaseManager.GameData gameData = dbManager.GetGold();
        UpdateGoldText(goldTextLocalized.GetLocalizedString());
        if (healthBar.GetCurrentHealth() <= 0)
        {
            DropGold();
            if (spawnController != null) // Kiểm tra controller còn tồn tại
            {
                spawnController.RemoveThrowerEnemy(gameObject); // Gọi hàm xóa lính khỏi danh sách
            }
        }
        // Kiểm tra nếu đang di chuyển
        if (isMoving)
        {
            Move();
        }
        else
        {
            // Nếu không còn đồng minh trong khoảng cách stopDistance, tiếp tục di chuyển
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, stopDistance);
            bool shouldMove = true;
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Ally") || hitCollider.CompareTag("AllyHall"))
                {
                    shouldMove = false;
                    break; // Thoát khỏi vòng lặp nếu tìm thấy đối tượng cần dừng
                }
            }

            if (shouldMove)
            {
                isMoving = true; // Cho phép di chuyển lại
                animator.SetInteger("Speed", 1); // Set tốc độ của animator
            }
        }
    }

    public void SetController(SpawnSoldierController controller) // Thiết lập controller
    {
        spawnController = controller;
    }
    public void TakeDamage(float amount)
    {
        healthBar.UpdateHealth(-amount);
    }

    void Move()
    {
        // Di chuyển từ phải sang trái
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Kiểm tra khoảng cách tới đối tượng có tag "Ally" hoặc "AllyHall"
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, stopDistance);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Ally") || hitCollider.CompareTag("AllyHall"))
            {
                // Dừng lại và set speed của animator = 0
                isMoving = false;
                animator.SetInteger("Speed", 0);

                // Ném đá nếu chưa ném trong thời gian cooldown
                if (Time.time > lastThrowTime + throwCooldown)
                {
                    StartCoroutine(ThrowRocks(hitCollider.gameObject)); // Bắt đầu ném đá liên tục
                }
                return; // Thoát khỏi hàm sau khi tìm thấy đối tượng trong khoảng cách
            }
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
        // Tạo đá và ném đến đồng minh
        GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity);
        RockEnemy rockComponent = rock.GetComponent<RockEnemy>();
        rockComponent.Initialize(target); // Chuyển vào đối tượng đồng minh
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Kiểm tra khi ra khỏi vùng va chạm
        if (other.CompareTag("Ally") || other.CompareTag("AllyHall"))
        {
            // Tiếp tục di chuyển và set speed của animator về lại
            isMoving = true;
            animator.SetInteger("Speed", 1);
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
