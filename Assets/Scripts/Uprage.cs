using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Uprage : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI goldUnlockSoldier2Text;
    public TextMeshProUGUI goldUnlockSoldier3Text;
    public TextMeshProUGUI goldUprageTimeText;
    public TextMeshProUGUI goldUprageHealthText;
    public Button btnUnlockSoldier2;
    public Button btnUnlockSoldier3;
    public Button btnUprageTime;
    public Button btnUprageHealth;
     public Image imgSoldier1;
    public Image imgSoldier2;
    public Image imgSoldier3;
    public Image imgSoldier1_1;
    public Image imgSoldier2_1;
    public Image imgSoldier3_1;
    public Image imgSoldier1_2;
    public Image imgSoldier2_2;
    public Image imgSoldier3_2;
    public TextMeshProUGUI healthText;
    private DatabaseManager dbManager;
    private BattleScript battleScript;
    public int id;
    private int mergeId;
    void Awake()
    {
        dbManager = FindObjectOfType<DatabaseManager>();
        battleScript = FindObjectOfType<BattleScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        id = battleScript.GetId();
    }

    // Update is called once per frame
    void Update()
    {
        if (id == 1){
            imgSoldier1_1.gameObject.SetActive(false);
            imgSoldier2_1.gameObject.SetActive(false);
            imgSoldier3_1.gameObject.SetActive(false);
            imgSoldier2_2.gameObject.SetActive(false);
            imgSoldier1_2.gameObject.SetActive(false);
            imgSoldier3_2.gameObject.SetActive(false);
            imgSoldier1.gameObject.SetActive(true);
            imgSoldier2.gameObject.SetActive(true);
            imgSoldier3.gameObject.SetActive(true);
            mergeId = 1;
        }
        if (id == 2|| id == 4){
            imgSoldier1.gameObject.SetActive(false);
            imgSoldier2.gameObject.SetActive(false);
            imgSoldier3.gameObject.SetActive(false);
            imgSoldier2_2.gameObject.SetActive(false);
            imgSoldier1_2.gameObject.SetActive(false);
            imgSoldier3_2.gameObject.SetActive(false);
            imgSoldier1_1.gameObject.SetActive(true);
            imgSoldier2_1.gameObject.SetActive(true);
            imgSoldier3_1.gameObject.SetActive(true);
            mergeId = 2;
        }
        if (id == 3|| id == 5 || id == 6){
            imgSoldier1.gameObject.SetActive(false);
            imgSoldier2.gameObject.SetActive(false);
            imgSoldier3.gameObject.SetActive(false);
            imgSoldier1_1.gameObject.SetActive(false);
            imgSoldier2_1.gameObject.SetActive(false);
            imgSoldier3_1.gameObject.SetActive(false);
            imgSoldier2_2.gameObject.SetActive(true);
            imgSoldier1_2.gameObject.SetActive(true);
            imgSoldier3_2.gameObject.SetActive(true);
            mergeId = 3;
        }
        DatabaseManager.GameData dataGold = dbManager.GetGold();
        DatabaseManager.Uprage dataUprage = dbManager.GetUprageById(mergeId);
        DatabaseManager.Hall dataHall = dbManager.GetHallById(mergeId);
        goldText.text = dataGold.Gold.ToString();
        healthText.text = dataHall.AllyHall.ToString();
        float result = 1 / float.Parse(dataUprage.TimeCooldownFood); // Kết quả sẽ là 3.3333f
        string formattedResult = result.ToString("F5");
        foodText.text = formattedResult +"/s";
        goldUprageTimeText.text = dataUprage.GoldUprageTime.ToString();
        goldUprageHealthText.text = dataUprage.GoldUprageHealthHall.ToString();
        goldUnlockSoldier2Text.text = dataUprage.GoldUprageAlly2.ToString();
        goldUnlockSoldier3Text.text = dataUprage.GoldUprageAlly3.ToString();
        if(dataGold.Gold < dataUprage.GoldUprageTime){
            btnUprageTime.interactable = false;
        }
        if(dataGold.Gold < dataUprage.GoldUprageHealthHall){
            btnUprageHealth.interactable = false;
        }
        if(dataGold.Gold < dataUprage.GoldUprageAlly2){
            btnUnlockSoldier2.interactable = false;
        }
        if(dataGold.Gold < dataUprage.GoldUprageAlly3){
            btnUnlockSoldier3.interactable = false;
        }
        if(dataUprage.LockAlly2 == 1){
            btnUnlockSoldier2.gameObject.SetActive(false);
        }
        if(dataUprage.LockAlly3 == 1){
            btnUnlockSoldier3.gameObject.SetActive(false);
        }
    }
}
