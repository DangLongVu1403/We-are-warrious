using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleScript : MonoBehaviour
{
    public TextMeshProUGUI goldText;
    private DatabaseManager dbManager;
    private Uprage uprage;
    private UprageManager uprageManager;
    public Button nextLevel;
    public Button backLevel;
    public int Id;
    void Awake()
    {
        dbManager = FindObjectOfType<DatabaseManager>();
        uprage = FindObjectOfType<Uprage>();
        uprageManager = FindObjectOfType<UprageManager>();
    }
    void Start()
    {
        Id=GetId();
        uprage.id = Id;
        if (Id >3){
            uprageManager.id = 3;
        }else if (Id==1){
            uprageManager.id = Id;
        } else{
            uprageManager.id = 2;
        }
    }
    void Update(){
        DatabaseManager.GameData dataGold = dbManager.GetGold();
        goldText.text = dataGold.Gold.ToString();
        if (Id == 1){
            nextLevel.gameObject.SetActive(false);
            backLevel.gameObject.SetActive(false);
        }
        if (Id == 2){
            backLevel.gameObject.SetActive(false);
            if (GetId() >= 3){
                nextLevel.gameObject.SetActive(true);
                nextLevel.onClick.AddListener(() => ChangeId(3));
            }else{
                nextLevel.gameObject.SetActive(false);
            }
        }
        if (Id == 4){
            backLevel.gameObject.SetActive(false);
            if (GetId() >= 5){
                nextLevel.gameObject.SetActive(true);
                nextLevel.onClick.AddListener(() => ChangeId(5));
            }else{
                nextLevel.gameObject.SetActive(false);
            }
        }
        if (Id == 3){
            nextLevel.gameObject.SetActive(false);
            backLevel.gameObject.SetActive(true);
            backLevel.onClick.AddListener(() => ChangeId(2));
        }
        if (Id ==5){
            backLevel.onClick.AddListener(() => ChangeId(4));
            if (GetId() > 5){
                nextLevel.gameObject.SetActive(true);
                nextLevel.onClick.AddListener(() => ChangeId(6));
            }else{
                nextLevel.gameObject.SetActive(false);
            }
            nextLevel.onClick.AddListener(() => ChangeId(6));
            backLevel.onClick.AddListener(() => ChangeId(4));
        }
        if (Id == 6){
            nextLevel.gameObject.SetActive(false);
            backLevel.onClick.AddListener(() => ChangeId(5));
        }
    }
     void ChangeId(int newId)
    {
        Id = newId;
        Debug.Log("Id đã được thay đổi thành: " + Id);
    }
    public void ChangeScene()
    {
        Time.timeScale = 1; // Khôi phục thời gian trò chơi nếu bị dừng
        if (Id == 1){
            SceneManager.LoadScene("BattleScene"); // Chuyển sang scene khác
        }
        if (Id == 2){
            SceneManager.LoadScene("2.1"); // Chuyển sang scene khác
        }
        if (Id == 4){
            SceneManager.LoadScene("3.1"); // Chuyển sang scene khác
        }
        if (Id == 3){
            SceneManager.LoadScene("2.2"); // Chuyển sang scene khác
        }
        if (Id == 5){
            SceneManager.LoadScene("3.2"); // Chuyển sang scene khác
        }
        if (Id == 6){
            SceneManager.LoadScene("3.3"); // Chuyển sang scene khác
        }
        PlayerPrefs.SetInt("Id", Id);
        AudioManager.Instance.ChangeMusic(AudioManager.Instance.Background);
    }

    public int GetId(){
    var levels = dbManager.GetAllLevels();

    Debug.Log("Số lượng level: " + levels.Count);

    // Duyệt qua từng bản ghi trong bảng Level
    for (int i = levels.Count - 1; i >= 0; i--)
    {
        DatabaseManager.Level level = levels[i]; // Lấy từng đối tượng Level
        Debug.Log("Checking level: " + level.Id + ", Pass: " + level.Pass);

        if ((level.Id == 6) && (level.Pass == 1)){
            Debug.Log("Returning Id 6");
            return 6;
        }
        if ((level.Id == 5) && (level.Pass == 1)){
            Debug.Log("Returning Id 5");
            return 5;
        }
        if ((level.Id == 4) && (level.Pass == 1)){
            Debug.Log("Returning Id 4");
            return 4;
        }
        if ((level.Id == 3) && (level.Pass == 1)){
            Debug.Log("Returning Id 3");
            return 3;
        }
        if ((level.Id == 2) && (level.Pass == 1)){
            Debug.Log("Returning Id 2");
            return 2;
        }
        if ((level.Id == 1) && (level.Pass == 1)){
            Debug.Log("Returning Id 1");
            return 1;
        }
    }

    Debug.Log("Returning default Id 1");
    return 1; // Giá trị mặc định nếu không có điều kiện nào thỏa mãn
}

}
