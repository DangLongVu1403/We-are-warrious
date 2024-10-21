using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodManager : MonoBehaviour
{
    public Slider cooldownSlider; // Thanh trượt cooldown
    public int id;
    public TextMeshProUGUI foodText; // Sử dụng TextMesh Pro
    public float cooldownTime = 1f; // Thời gian cooldown (mặc định nếu không lấy được từ database)
    private float currentCooldown; // Thời gian hiện tại của cooldown
    private int food = 0; // Lượng thực phẩm

    private DatabaseManager dbManager; // Biến tham chiếu đến DatabaseManager
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        id = gameManager.getIdAlly();
        // Lấy tham chiếu đến DatabaseManager ngay khi bắt đầu
        dbManager = FindObjectOfType<DatabaseManager>();

        // Lấy dữ liệu từ database khi bắt đầu nếu có
        if (dbManager != null)
        {
            DatabaseManager.Uprage data = dbManager.GetUprageById(id); // Sử dụng kiểu đầy đủ của GameData
            if (data != null)
            {
                // Kiểm tra xem chuỗi có thể chuyển thành float hay không và lấy giá trị
                if (float.TryParse(data.TimeCooldownFood, out float parsedCooldownTime))
                {
                    cooldownTime = parsedCooldownTime;
                }
                else
                {
                    Debug.LogWarning("Failed to parse cooldown time from database.");
                }
            }
        }

        currentCooldown = 0; // Khởi tạo cooldown ở 0
        cooldownSlider.maxValue = cooldownTime; // Thiết lập giá trị tối đa cho thanh trượt
        cooldownSlider.value = currentCooldown; // Thiết lập giá trị khởi đầu cho thanh trượt
    }


    void Update()
    {
        // Tăng giá trị thanh trượt theo thời gian
        if (currentCooldown < cooldownTime)
        {
            currentCooldown += Time.deltaTime; // Tăng dần
            cooldownSlider.value = currentCooldown; // Cập nhật giá trị thanh trượt

            // Kiểm tra nếu thanh trượt đầy
            if (currentCooldown >= cooldownTime)
            {
                food++; // Tăng lượng thực phẩm
                UpdateFoodText(); // Cập nhật hiển thị thực phẩm
                currentCooldown = 0; // Reset cooldown
            }
        }
    }

    // Cập nhật văn bản hiển thị thực phẩm
    void UpdateFoodText()
    {
        foodText.text = food.ToString(); // Cập nhật văn bản hiển thị lượng thực phẩm
    }

    public void Updatefood(int ConsumedFood){
        food -= ConsumedFood;
    }
}
