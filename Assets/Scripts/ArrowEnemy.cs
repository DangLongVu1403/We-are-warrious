using UnityEngine;

public class ArrowEnemy : MonoBehaviour
{
    public float speed = 5f;
    public float damage = 20f;
    [SerializeField] private float lifeTime = 2f;
    private GameObject target;
    private DatabaseManager databaseManager;
    private GameManager gameManager;
    private int id;

    void Awake()
    {
        databaseManager = FindObjectOfType<DatabaseManager>();
        gameManager = FindObjectOfType<GameManager>();

        transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        id = gameManager.getIdEnemy();
    }

    void Start()
    {
        DatabaseManager.SoldierEnemy data = databaseManager.GetSoldierEnemyById(id);
        damage = data.DamageEnemy2;
    }

    public void Initialize(GameObject target)
    {
        this.target = target;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {

        if (target != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }

            if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
            {
                Debug.Log("khanhs" + "in zone" + $"{target.gameObject.tag}");
                if (target.CompareTag("Ally"))
                {
                    AllyMovement ally = target.GetComponent<AllyMovement>();
                    if (ally != null)
                    {
                        ally.TakeDamage(damage);
                    }
                    ThrowerAlly throwerAlly = target.GetComponent<ThrowerAlly>();
                    if (throwerAlly != null)
                    {
                        throwerAlly.TakeDamage(damage);
                    }
                    ArcherAlly archerAlly = target.GetComponent<ArcherAlly>();
                    if (archerAlly != null)
                    {
                        archerAlly.TakeDamage(damage);
                    }
                    Ally3Movement ally3 = target.GetComponent<Ally3Movement>();
                    if (ally3 != null)
                    {
                        ally3.TakeDamage(damage);
                    }
                }
                else if (target.CompareTag("AllyHall"))
                    {
                        Debug.Log("khanhs" + "equal");
                        AllyHall allyHall = target.GetComponent<AllyHall>();
                        if (allyHall != null)
                        {
                            Debug.Log("khanhs" + "!= null");
                            allyHall.TakeDamage(damage);
                        }
                    }

                Destroy(gameObject);
            }
        }
    }
}
