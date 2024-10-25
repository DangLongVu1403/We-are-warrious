using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SpawnSoldierController : MonoBehaviour
{
    public GameObject allyPrefab; // Prefab của lính đồng minh
    public GameObject archerAllyPrefab;
     public GameObject archerEnemyPrefab;
    public GameObject enemyPrefab; // Prefab của lính kẻ địch
    public GameObject ally3Prefab; // Prefab của lính đồng minh
    public GameObject enemy3Prefab; // Prefab của lính kẻ địch
    public GameObject throwerAllyPrefab; // Prefab của lính ném đá đồng minh
    public GameObject throwerEnemyPrefab; // Prefab của lính ném đá kẻ địch
    public Transform allySpawnPoint; // Vị trí spawn lính đồng minh
    public Transform enemySpawnPoint; // Vị trí spawn lính kẻ địch
    public Transform throwerAllySpawnPoint; // Vị trí spawn lính ném đá đồng minh
    public Transform throwerEnemySpawnPoint; // Vị trí spawn lính ném đá kẻ địch

    public List<GameObject> allySoldiers = new List<GameObject>(); // Danh sách lính đồng minh
    public List<GameObject> enemySoldiers = new List<GameObject>(); // Danh sách lính kẻ địch
    public List<GameObject> throwerAllies = new List<GameObject>(); // Danh sách lính ném đá đồng minh
    public List<GameObject> throwerEnemies = new List<GameObject>(); // Danh sách lính ném đá kẻ địch

    public TextMeshProUGUI foodText; // TextMeshPro cho số lượng thực phẩm hiện có
    public TextMeshProUGUI neededFoodAlly1Text; // TextMeshPro cho số lượng thực phẩm cần thiết để spawn lính đồng minh
    public TextMeshProUGUI neededFoodAlly2Text; // TextMeshPro cho số lượng thực phẩm cần thiết để spawn lính kẻ địch
    public TextMeshProUGUI neededFoodAlly3Text; // TextMeshPro cho số lượng thực phẩm cần thiết để spawn lính ném đá
    public Button spawnAlly1Button; // Nút spawn lính đồng minh
    public Button spawnAlly2Button; // Nút spawn lính kẻ địch
    public Button spawnAlly3Button; // Nút spawn lính ném đá
    private DatabaseManager databaseManager;
    private GameManager gameManager;
    private FoodManager foodManager;
    private int id;

    void Awake(){
        databaseManager = FindObjectOfType<DatabaseManager>();
        gameManager = FindObjectOfType<GameManager>();
        foodManager = FindObjectOfType<FoodManager>();
    }

    private void Start()
    {
        id = gameManager.getIdEnemy();
        StartCoroutine(SpawnEnemySoldierCoroutine1());
        if (id == 1 || id ==3){
            StartCoroutine(SpawnEnemySoldierCoroutine2());  
        }
        else if(id==2){
            StartCoroutine(SpawnEnemySoldierCoroutine2_1());
        }
        StartCoroutine(SpawnEnemySoldierCoroutine3());


        // // Ẩn tất cả các nút spawn khi bắt đầu
        // spawnAllyButton.gameObject.SetActive(false);
        // spawnEnemyButton.gameObject.SetActive(false);
        // spawnThrowerButton.gameObject.SetActive(false);
        UpdateSpawnButtons();
    }

    private void Update(){
        int currentFood = int.Parse(foodText.text);
        int neededFoodAlly1 = int.Parse(neededFoodAlly1Text.text);
        int neededFoodAlly2 = int.Parse(neededFoodAlly2Text.text);
        int neededFoodAlly3 = int.Parse(neededFoodAlly3Text.text);
        if (currentFood< neededFoodAlly1){
            spawnAlly1Button.interactable = false;
        }else{
            spawnAlly1Button.interactable = true;
        }
        if (currentFood< neededFoodAlly2){
            spawnAlly2Button.interactable = false;
        }else{
            spawnAlly2Button.interactable = true;
        }
        if (currentFood< neededFoodAlly3){
            spawnAlly3Button.interactable = false;
        }else{
            spawnAlly2Button.interactable = true;
        }
    }

    private IEnumerator SpawnEnemySoldierCoroutine1()
    {
        var spawnData = databaseManager.GetSpawnSoldier1ById(id); // Lấy dữ liệu từ bảng SpawnSoldier1

        if (spawnData != null)
        {
            float elapsedTime = 0f; // Thời gian đã trôi qua
            float nextSpawnTime = spawnData.Time1; // Thời gian đến lần spawn tiếp theo
            int currentQuantity = spawnData.Quantity1; // Số lượng lính muốn spawn

            while (true)
            {
                elapsedTime += Time.deltaTime; // Cập nhật thời gian đã trôi qua

                if (elapsedTime >= nextSpawnTime)
                {
                    // Spawn lính mới theo số lượng được chỉ định
                    for (int i = 0; i < currentQuantity; i++)
                    {
                        // Lấy vị trí spawn hợp lệ
                        Vector3 spawnPosition = GetValidSpawnPosition(enemySpawnPoint.position, enemySoldiers);

                        // Tạo lính mới
                        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, enemySpawnPoint.rotation);
                        newEnemy.GetComponent<EnemyMovement>().SetController(this); // Thiết lập controller cho lính mới
                        enemySoldiers.Add(newEnemy); // Thêm lính vào danh sách enemySoldiers
                    }

                    // Cập nhật thời gian và số lượng cho lần spawn tiếp theo
                    if (nextSpawnTime == spawnData.Time1)
                    {
                        nextSpawnTime = spawnData.Time2;
                        currentQuantity = spawnData.Quantity2;
                    }
                    else if (nextSpawnTime == spawnData.Time2)
                    {
                        nextSpawnTime = spawnData.Time3;
                        currentQuantity = spawnData.Quantity3;
                    }
                    else
                    {
                        // Reset lại nếu đã đến cuối
                        nextSpawnTime = spawnData.Time1;
                        currentQuantity = spawnData.Quantity1;
                    }

                    elapsedTime = 0f; // Reset thời gian đã trôi qua
                }

                yield return null; // Đợi khung hình tiếp theo
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy dữ liệu SpawnSoldier1 cho ID 1.");
        }
    }



     private IEnumerator SpawnEnemySoldierCoroutine2()
    {
        var spawnData = databaseManager.GetSpawnSoldier2ById(id); // Lấy dữ liệu từ bảng SpawnSoldier1

        if (spawnData != null)
        {
            float elapsedTime = 0f; // Thời gian đã trôi qua
            float nextSpawnTime = spawnData.Time1; // Thời gian đến lần spawn tiếp theo
            int currentQuantity = spawnData.Quantity1; // Số lượng lính muốn spawn

            while (true)
            {
                elapsedTime += Time.deltaTime; // Cập nhật thời gian đã trôi qua

                if (elapsedTime >= nextSpawnTime)
                {
                    // Spawn lính mới theo số lượng được chỉ định
                    for (int i = 0; i < currentQuantity; i++)
                    {
                        // Lấy vị trí spawn hợp lệ
                        Vector3 spawnPosition = GetValidSpawnPosition(enemySpawnPoint.position, throwerEnemies);

                        // Tạo lính mới
                        GameObject newEnemy = Instantiate(throwerEnemyPrefab, spawnPosition, enemySpawnPoint.rotation);
                        newEnemy.GetComponent<ThrowerEnemy>().SetController(this); // Thiết lập controller cho lính mới
                        throwerEnemies.Add(newEnemy); // Thêm lính vào danh sách enemySoldiers
                    }

                    // Cập nhật thời gian và số lượng cho lần spawn tiếp theo
                    if (nextSpawnTime == spawnData.Time1)
                    {
                        nextSpawnTime = spawnData.Time2;
                        currentQuantity = spawnData.Quantity2;
                    }
                    else if (nextSpawnTime == spawnData.Time2)
                    {
                        nextSpawnTime = spawnData.Time3;
                        currentQuantity = spawnData.Quantity3;
                    }
                    else
                    {
                        // Reset lại nếu đã đến cuối
                        nextSpawnTime = spawnData.Time1;
                        currentQuantity = spawnData.Quantity1;
                    }

                    elapsedTime = 0f; // Reset thời gian đã trôi qua
                }

                yield return null; // Đợi khung hình tiếp theo
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy dữ liệu SpawnSoldier1 cho ID 1.");
        }
    }

    private IEnumerator SpawnEnemySoldierCoroutine2_1()
    {
        var spawnData = databaseManager.GetSpawnSoldier2ById(id); // Lấy dữ liệu từ bảng SpawnSoldier1

        if (spawnData != null)
        {
            float elapsedTime = 0f; // Thời gian đã trôi qua
            float nextSpawnTime = spawnData.Time1; // Thời gian đến lần spawn tiếp theo
            int currentQuantity = spawnData.Quantity1; // Số lượng lính muốn spawn

            while (true)
            {
                elapsedTime += Time.deltaTime; // Cập nhật thời gian đã trôi qua

                if (elapsedTime >= nextSpawnTime)
                {
                    // Spawn lính mới theo số lượng được chỉ định
                    for (int i = 0; i < currentQuantity; i++)
                    {
                        // Lấy vị trí spawn hợp lệ
                        Vector3 spawnPosition = GetValidSpawnPosition(enemySpawnPoint.position, throwerEnemies);

                        // Tạo lính mới
                        GameObject newEnemy = Instantiate(archerEnemyPrefab, spawnPosition, enemySpawnPoint.rotation);
                        newEnemy.GetComponent<ArcherEnemy>().SetController(this); // Thiết lập controller cho lính mới
                        throwerEnemies.Add(newEnemy); // Thêm lính vào danh sách enemySoldiers
                    }

                    // Cập nhật thời gian và số lượng cho lần spawn tiếp theo
                    if (nextSpawnTime == spawnData.Time1)
                    {
                        nextSpawnTime = spawnData.Time2;
                        currentQuantity = spawnData.Quantity2;
                    }
                    else if (nextSpawnTime == spawnData.Time2)
                    {
                        nextSpawnTime = spawnData.Time3;
                        currentQuantity = spawnData.Quantity3;
                    }
                    else
                    {
                        // Reset lại nếu đã đến cuối
                        nextSpawnTime = spawnData.Time1;
                        currentQuantity = spawnData.Quantity1;
                    }

                    elapsedTime = 0f; // Reset thời gian đã trôi qua
                }

                yield return null; // Đợi khung hình tiếp theo
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy dữ liệu SpawnSoldier1 cho ID 1.");
        }
    }
     private IEnumerator SpawnEnemySoldierCoroutine3()
    {
        var spawnData = databaseManager.GetSpawnSoldier3ById(id); // Lấy dữ liệu từ bảng SpawnSoldier1

        if (spawnData != null)
        {
            float elapsedTime = 0f; // Thời gian đã trôi qua
            float nextSpawnTime = spawnData.Time1; // Thời gian đến lần spawn tiếp theo
            int currentQuantity = spawnData.Quantity1; // Số lượng lính muốn spawn

            while (true)
            {
                elapsedTime += Time.deltaTime; // Cập nhật thời gian đã trôi qua

                if (elapsedTime >= nextSpawnTime)
                {
                    // Spawn lính mới theo số lượng được chỉ định
                    for (int i = 0; i < currentQuantity; i++)
                    {
                        // Lấy vị trí spawn hợp lệ
                        Vector3 spawnPosition = GetValidSpawnPosition(enemySpawnPoint.position, enemySoldiers);

                        // Tạo lính mới
                        GameObject newEnemy = Instantiate(enemy3Prefab, spawnPosition, enemySpawnPoint.rotation);
                        newEnemy.GetComponent<Enemy3Movement>().SetController(this); // Thiết lập controller cho lính mới
                        enemySoldiers.Add(newEnemy); // Thêm lính vào danh sách enemySoldiers
                    }

                    // Cập nhật thời gian và số lượng cho lần spawn tiếp theo
                    if (nextSpawnTime == spawnData.Time1)
                    {
                        nextSpawnTime = spawnData.Time2;
                        currentQuantity = spawnData.Quantity2;
                    }
                    else if (nextSpawnTime == spawnData.Time2)
                    {
                        nextSpawnTime = spawnData.Time3;
                        currentQuantity = spawnData.Quantity3;
                    }
                    else
                    {
                        // Reset lại nếu đã đến cuối
                        nextSpawnTime = spawnData.Time1;
                        currentQuantity = spawnData.Quantity1;
                    }

                    elapsedTime = 0f; // Reset thời gian đã trôi qua
                }

                yield return null; // Đợi khung hình tiếp theo
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy dữ liệu SpawnSoldier1 cho ID 1.");
        }
    }
    private void UpdateSpawnButtons()
    {
        // Check if all TextMeshProUGUI references are assigned
        if (foodText == null || neededFoodAlly1Text == null || neededFoodAlly2Text == null || neededFoodAlly3Text == null)
        {
            Debug.LogError("One or more TextMeshProUGUI references are not assigned!");
            return; // Exit the method to prevent further errors
        }

        // Lấy giá trị số lượng thực phẩm hiện có
        int currentFood = int.Parse(foodText.text);
        int neededFoodAlly1 = int.Parse(neededFoodAlly1Text.text);
        int neededFoodAlly2 = int.Parse(neededFoodAlly2Text.text);
        int neededFoodAlly3 = int.Parse(neededFoodAlly3Text.text);

        // Cập nhật trạng thái nút spawn
        UpdateButtonAppearance(spawnAlly1Button, currentFood >= neededFoodAlly1);
        UpdateButtonAppearance(spawnAlly2Button, currentFood >= neededFoodAlly2);
        UpdateButtonAppearance(spawnAlly3Button, currentFood >= neededFoodAlly3);
    }

    // Hàm cập nhật màu sắc cho nút
    private void UpdateButtonAppearance(Button button, bool isEnabled)
    {
        ColorBlock colorBlock = button.colors;

        if (isEnabled)
        {
            // Nếu đủ thực phẩm, sử dụng màu mặc định
            colorBlock.normalColor = Color.white; // Màu mặc định
            colorBlock.highlightedColor = Color.grey; // Màu khi rê chuột lên
        }
        button.colors = colorBlock; // Cập nhật màu cho nút
    }

    public void SpawnAllySoldier()
    {
        if (CanSpawnSoldier(neededFoodAlly1Text))
        {
            Vector3 spawnPosition = GetValidSpawnPosition(allySpawnPoint.position, allySoldiers);
            GameObject newAlly = Instantiate(allyPrefab, spawnPosition, allySpawnPoint.rotation);
            newAlly.GetComponent<AllyMovement>().SetController(this); // Thiết lập controller cho lính mới
            allySoldiers.Add(newAlly);
            UpdateFoodCount(neededFoodAlly1Text); // Cập nhật số lượng thực phẩm sau khi spawn
        }
    }
    public void SpawnArrowSoldier()
    {
        if (CanSpawnSoldier(neededFoodAlly2Text))
        {
            Vector3 spawnPosition = GetValidSpawnPosition(allySpawnPoint.position, throwerAllies);
            GameObject newAlly = Instantiate(archerAllyPrefab, spawnPosition, allySpawnPoint.rotation);
            newAlly.GetComponent<ArcherAlly>().SetController(this); // Thiết lập controller cho lính mới
            throwerAllies.Add(newAlly);
            UpdateFoodCount(neededFoodAlly2Text); // Cập nhật số lượng thực phẩm sau khi spawn
        }
    }

    public void SpawnAlly3Soldier()
    {
        if (CanSpawnSoldier(neededFoodAlly3Text))
        {
            Vector3 spawnPosition = GetValidSpawnPosition(allySpawnPoint.position, allySoldiers);
            GameObject newAlly = Instantiate(ally3Prefab, spawnPosition, allySpawnPoint.rotation);
            newAlly.GetComponent<Ally3Movement>().SetController(this); // Thiết lập controller cho lính kẻ địch mới
            allySoldiers.Add(newAlly);
            UpdateFoodCount(neededFoodAlly3Text); // Cập nhật số lượng thực phẩm sau khi spawn
        }
    }

    public void SpawnThrowerAlly()
    {
        if (CanSpawnSoldier(neededFoodAlly2Text))
        {
            Vector3 spawnPosition = GetValidSpawnPosition(throwerAllySpawnPoint.position, throwerAllies);
            GameObject newThrowerAlly = Instantiate(throwerAllyPrefab, spawnPosition, throwerAllySpawnPoint.rotation);
            newThrowerAlly.GetComponent<ThrowerAlly>().SetController(this); // Thiết lập controller cho lính ném đá mới
            throwerAllies.Add(newThrowerAlly);
            UpdateFoodCount(neededFoodAlly2Text); // Cập nhật số lượng thực phẩm sau khi spawn
        }
    }

    // Hàm kiểm tra xem có đủ thực phẩm để spawn hay không
    private bool CanSpawnSoldier(TextMeshProUGUI neededFoodText)
    {
        int currentFood = int.Parse(foodText.text);
        int neededFood = int.Parse(neededFoodText.text);
        return currentFood >= neededFood;
    }

    // Cập nhật số lượng thực phẩm sau khi spawn
    private void UpdateFoodCount(TextMeshProUGUI neededFoodText)
    {
        int currentFood = int.Parse(foodText.text);
        int neededFood = int.Parse(neededFoodText.text);

        // Giảm số lượng thực phẩm sau khi spawn
        currentFood -= neededFood;
        foodText.text = currentFood.ToString();

        UpdateSpawnButtons(); // Cập nhật trạng thái các nút spawn
        foodManager.Updatefood(neededFood);
    }

    // Hàm kiểm tra và trả về vị trí spawn hợp lệ (tránh va chạm với các lính khác)
    private Vector3 GetValidSpawnPosition(Vector3 originalPosition, List<GameObject> allSoldiers)
    {
        float offset = 0.1f; // Khoảng cách cần tránh giữa các lính
        Vector3 newPosition = originalPosition;

        bool positionIsValid = false;
        int maxAttempts = 10; // Giới hạn số lần thử tìm vị trí mới
        int attempt = 0;

        while (!positionIsValid && attempt < maxAttempts)
        {
            positionIsValid = true;

            foreach (GameObject soldier in allSoldiers)
            {
                if (soldier != null)
                {
                    float distance = Vector3.Distance(newPosition, soldier.transform.position);
                    if (distance < offset)
                    {
                        newPosition.y += offset; // Điều chỉnh vị trí y nếu bị trùng
                        positionIsValid = false;
                        break;
                    }
                }
            }

            attempt++;
        }

        return newPosition; // Trả về vị trí mới hợp lệ
    }

    // Các hàm xóa lính
    public void RemoveAllySoldier(GameObject soldier)
    {
        if (allySoldiers.Contains(soldier))
        {
            allySoldiers.Remove(soldier); // Xóa lính khỏi danh sách
            Destroy(soldier);
        }
    }

    public void RemoveEnemySoldier(GameObject soldier)
    {
        if (enemySoldiers.Contains(soldier))
        {
            enemySoldiers.Remove(soldier); // Xóa lính khỏi danh sách
            Destroy(soldier);
        }
    }

    public void RemoveThrowerAlly(GameObject thrower)
    {
        if (throwerAllies.Contains(thrower))
        {
            throwerAllies.Remove(thrower); // Xóa lính ném đá khỏi danh sách
            Destroy(thrower);
        }
    }

    public void RemoveThrowerEnemy(GameObject thrower)
    {
        if (throwerEnemies.Contains(thrower))
        {
            throwerEnemies.Remove(thrower); // Xóa lính ném đá khỏi danh sách
            Destroy(thrower);
        }
    }

    // Hàm trả về tất cả lính đồng minh
    public List<GameObject> GetAllAllySoldiers()
    {
        return allySoldiers;
    }

    // Hàm trả về tất cả lính kẻ địch
    public List<GameObject> GetAllEnemySoldiers()
    {
        return enemySoldiers;
    }

    // Hàm trả về tất cả lính ném đá đồng minh
    public List<GameObject> GetAllThrowerAllies()
    {
        return throwerAllies;
    }

    // Hàm trả về tất cả lính ném đá kẻ địch
    public List<GameObject> GetAllThrowerEnemies()
    {
        return throwerEnemies;
    }
}
