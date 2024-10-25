using UnityEngine;
using System.Collections;

public class AllyMovement : MonoBehaviour
{
    public float speed = 0.4f;
    public float damage = 20f;
    public int id;
    public HealthBar healthBar;
    private Animator animator;
    private bool isColliding = false; // Biến để theo dõi va chạm
    private Coroutine damageCoroutine; // Coroutine để trừ máu liên tục
    private SpawnSoldierController spawnController; // Tham chiếu đến controller quản lý spawn
    private float separationDistance = 0.2f; // Khoảng cách tách nhau giữa các lính
    private DatabaseManager dbManager;
    private GameManager gameManager;
    void Awake(){
        gameManager = FindObjectOfType<GameManager>();
        dbManager = FindObjectOfType<DatabaseManager>();
        id = gameManager.getIdAlly();
        if(id == 1){
            transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        } else if(id == 2){
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        } else if(id == 3){
            transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        if (healthBar != null) // Kiểm tra xem healthBar có được gán không
        {
            DatabaseManager.SoldierAlly data = dbManager.GetSoldierAllyById(id);
            if (dbManager != null){
                healthBar.SetMaxHealth(data.HealthAlly1);
                damage = data.DamageAlly1;
            }
        }
    }

    public void SetController(SpawnSoldierController controller) // Thiết lập controller
    {
        spawnController = controller;
    }

    void Update()
    {
        // Kiểm tra xem lính có quá gần lính khác không và tách ra
        SeparateFromAllies();

        if (!isColliding) // Nếu không va chạm
        {
            animator.SetInteger("Speed", 1); // Di chuyển
            transform.Translate(Vector3.right * speed * Time.deltaTime);
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
            if (spawnController != null) // Kiểm tra controller còn tồn tại
            {
                spawnController.RemoveAllySoldier(gameObject); // Gọi hàm xóa lính khỏi danh sách
            }
        }
    }


    public void TakeDamage(float amount)
    {
        healthBar.UpdateHealth(-amount);
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isColliding = true;
            animator.SetInteger("Speed", 0);

            if (damageCoroutine == null) 
            {
                damageCoroutine = StartCoroutine(DealDamageOverTime(other.gameObject));
            }
        }
        else if (other.gameObject.CompareTag("EnemyHall")) // Kiểm tra va chạm với Hall
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
        Hall hall = hallObj.GetComponent<Hall>(); 
        EnemyHall enemyHall = hallObj.GetComponent<EnemyHall>();
        while (hall != null && isColliding)
        {
            yield return new WaitForSeconds(0.5f);
            hall.TakeDamage(damage); // Gây sát thương cho Hall
            enemyHall.DropGold();
        }
    }



    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("EnemyHall"))
        {
            isColliding = false; // Không còn va chạm
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine); // Dừng Coroutine khi không còn va chạm
                damageCoroutine = null;
            }
        }
    }

    IEnumerator DealDamageOverTime(GameObject enemyObj)
    {
        EnemyMovement enemy = enemyObj.GetComponent<EnemyMovement>();
        while (enemy != null && isColliding)
        {
            yield return new WaitForSeconds(0.5f);
            if (enemy != null) // Kiểm tra enemy có còn tồn tại không
            {
                enemy.TakeDamage(damage);
            } 
        }
        Enemy3Movement enemy3 = enemyObj.GetComponent<Enemy3Movement>();
        while (enemy3 != null && isColliding)
        {
            yield return new WaitForSeconds(0.5f);
            if (enemy3 != null) // Kiểm tra enemy có còn tồn tại không
            {
                enemy3.TakeDamage(damage);
            }
        }
        ArcherEnemy enemy21 = enemyObj.GetComponent<ArcherEnemy>();
        while (enemy21 != null && isColliding)
        {
            yield return new WaitForSeconds(0.5f);
            if (enemy21 != null) // Kiểm tra enemy có còn tồn tại không
            {
                enemy21.TakeDamage(damage);
            }
        }
        ThrowerEnemy enemy2 = enemyObj.GetComponent<ThrowerEnemy>();
        while (enemy2 != null && isColliding)
        {
            yield return new WaitForSeconds(0.5f);
            if (enemy2 != null) // Kiểm tra enemy có còn tồn tại không
            {
                enemy2.TakeDamage(damage);
            }
        }
    }

    IEnumerator DealDamageToHall(GameObject hallObj)
    {
        Hall hall = hallObj.GetComponent<Hall>();
        while (hall != null && isColliding)
        {
            yield return new WaitForSeconds(0.5f);
            hall.TakeDamage(damage); // Gây damage cho hall
        }
    }

    void SeparateFromAllies()
    {
        AllyMovement[] allies = FindObjectsOfType<AllyMovement>(); // Lấy tất cả lính đồng minh

        foreach (AllyMovement ally in allies)
        {
            if (ally != this) // Không tự kiểm tra bản thân
            {
                float distance = Vector3.Distance(transform.position, ally.transform.position);
                if (distance < separationDistance) // Nếu khoảng cách nhỏ hơn khoảng tách
                {
                    // Tính toán khoảng cách cần tách
                    float offsetY = transform.position.y > ally.transform.position.y ? -separationDistance : separationDistance;
                    Vector3 newPosition = new Vector3(transform.position.x, transform.position.y + offsetY, transform.position.z);
                    
                    // Giới hạn vị trí Y trong khoảng -0.3f đến 0.3f
                    newPosition.y = Mathf.Clamp(newPosition.y, -0.1f, 0.5f);

                    transform.position = newPosition; // Cập nhật vị trí
                }
            }
        }
    }

}
