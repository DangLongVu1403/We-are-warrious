using UnityEngine;

public class AllyHall : MonoBehaviour
{
    [SerializeField] private GameObject graphic;
    [SerializeField] private Vector2 offsetGraphic;
    public HealthBar healthBar; // Tham chiếu đến HealthBar
    public int id;
    private float currentHealth; // Sức khỏe hiện tại của hall
    private GameManager gameManager; 
    private DatabaseManager dbManager;

    void Awake()
    {
        dbManager = FindObjectOfType<DatabaseManager>();
        gameManager = FindObjectOfType<GameManager>(); 

        AlignToLeft();
    }
    void Start(){
        if (healthBar != null) // Kiểm tra xem healthBar có được gán không
        {
            DatabaseManager.Hall data = dbManager.GetHallById(id);
        if (dbManager != null){
                healthBar.SetMaxHealth(data.AllyHall);
                currentHealth = data.AllyHall; 
            }
        }
        else
        {
            Debug.LogError("HealthBar is not assigned in the Inspector.");
        }
        id = gameManager.getIdAlly();
    }
     void Update()
    {
        if (healthBar.GetCurrentHealth() <= 0){
            gameManager.DisplayDefeatMessage();
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Giảm sức khỏe
        healthBar.UpdateHealth(-amount); 

        if (currentHealth <= 0)
        {
            currentHealth = 0; // Đảm bảo sức khỏe không âm
            DisplayLoseMessage(); // Hiển thị thông báo thất bại
        }
    }

    public void DisplayLoseMessage()
    {
        Time.timeScale = 0; // Dừng thời gian trò chơi
    }

    public bool IsDefeated()
    {
        return currentHealth <= 0;
    }

    private void AlignToLeft()
    {
        Camera mainCamera = Camera.main;
        Vector3 leftScreenPosition = mainCamera.ScreenToWorldPoint(new Vector2(0f, Screen.height / 2));
        leftScreenPosition += (Vector3) offsetGraphic;
        graphic.transform.position = new Vector3(leftScreenPosition.x, transform.position.y, transform.position.z);
    }
}
