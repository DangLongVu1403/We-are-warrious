using UnityEngine;
using UnityEngine.Localization;
using TMPro;

public class EnemyHall : MonoBehaviour
{
    public HealthBar healthBar; // Tham chiếu đến HealthBar
    public int id;
    private float currentHealth; // Sức khỏe hiện tại của hall
    private GameManager gameManager; // Tham chiếu đến GameManager
    private DatabaseManager dbManager;
    public GameObject goldPrefab;
    public int goldAmount = 1;
    public Transform goldTarget;
    public LocalizedString goldTextLocalized;
    public TextMeshProUGUI goldText;

    void Awake()
    {
        dbManager = FindObjectOfType<DatabaseManager>();
        gameManager = FindObjectOfType<GameManager>(); 
        if (healthBar != null) // Kiểm tra xem healthBar có được gán không
        {
            DatabaseManager.Hall data = dbManager.GetHallById(id);
        if (dbManager != null){
                healthBar.SetMaxHealth(data.EnemyHall);
                currentHealth = data.EnemyHall ; 
            }
        }
        else
        {
            Debug.LogError("HealthBar is not assigned in the Inspector.");
        }
        id = gameManager.getIdEnemy();
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
        UpdateGoldText(goldTextLocalized.GetLocalizedString());
        if (healthBar.GetCurrentHealth() <= 0){
            gameManager.DisplayVictoryMessage();
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Giảm sức khỏe
        healthBar.UpdateHealth(-amount);
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Đảm bảo sức khỏe không âm
            gameManager.DisplayVictoryMessage(); // Gọi GameManager để hiển thị chiến thắng
        }
    }

    public bool IsDefeated()
    {
        dbManager.UpdateLevel(PlayerPrefs.GetInt("Id"),1);
        return currentHealth <= 0;
    }
    public void DropGold()
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
