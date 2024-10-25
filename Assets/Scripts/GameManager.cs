using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Data.Common;

public class GameManager : MonoBehaviour
{
    public AllyHall allyHall; // Tham chiếu đến AllyHall
    public EnemyHall enemyHall; // Tham chiếu đến EnemyHall
    public GameObject infoPanel; // Panel chứa thông tin cần ẩn/hiện
    public Image resultImage; // UI Image để hiển thị kết quả (victory/defeat)
    public Sprite victorySprite; // Hình ảnh cho chiến thắng
    public Sprite defeatSprite; // Hình ảnh cho thất bại
    public GameObject PanelEnd;
    public int id;

    public TextMeshProUGUI goldText;

    private DatabaseManager dbManager;

    private bool gameOver = false;
    public Button btnSpawnSoldier2;
    public Button btnSpawnSoldier3;

    void Awake()
    {
        infoPanel.SetActive(false); // Ẩn panel khi bắt đầu
        PanelEnd.SetActive(false);
        dbManager = FindObjectOfType<DatabaseManager>();
        id = PlayerPrefs.GetInt("Id");
    }

    public int getIdAlly(){
        if ( PlayerPrefs.GetInt("Id") == 3 || PlayerPrefs.GetInt("Id") == 2 ){
            return 2;
        }else if (PlayerPrefs.GetInt("Id") == 5 || PlayerPrefs.GetInt("Id") == 6 || PlayerPrefs.GetInt("Id") == 4){
            return 3;
        }else{
            return 1;
        }
    }

    public int getIdEnemy(){
        if ( PlayerPrefs.GetInt("Id") == 6 ){
            return 3;
        }else if (PlayerPrefs.GetInt("Id") == 3 || PlayerPrefs.GetInt("Id") == 5){
            return 2;
        }else{
            return 1;
        }
    }

    void Update()
    {
        DatabaseManager.Uprage dataUprage = dbManager.GetUprageById(getIdAlly());
        DatabaseManager.GameData data = dbManager.GetGold();
        goldText.text = data.Gold.ToString();
        if (!gameOver)
        {
            // Kiểm tra máu của AllyHall và EnemyHall
            if (allyHall.IsDefeated())
            {
                Debug.Log("Ally Hall defeated!");
                DisplayDefeatMessage();
            }
            else if (enemyHall.IsDefeated())
            {
                Debug.Log("Enemy Hall defeated!");
                DisplayVictoryMessage();
            }
        }
        if (dataUprage.LockAlly2 == 0){
            btnSpawnSoldier2.interactable = false;
        }else{
            btnSpawnSoldier2.interactable = true;
        }
        if (dataUprage.LockAlly3 == 0){
            btnSpawnSoldier3.interactable = false;
        }else{
            btnSpawnSoldier3.interactable = true;
        }
    }

    // Hàm hiển thị thông báo chiến thắng
    public void DisplayVictoryMessage()
    {
        if (!gameOver) // Đảm bảo chỉ hiển thị kết quả một lần
        {
            resultImage.sprite = victorySprite;
            Debug.Log("Victory message displayed!");
            if (PlayerPrefs.GetInt("Id") == 1){
                dbManager.UpdateLevel(2,1);
            }else if (PlayerPrefs.GetInt("Id") == 2){
                dbManager.UpdateLevel(3,1);
            }else if (PlayerPrefs.GetInt("Id") == 3){
                dbManager.UpdateLevel(4,1);
            }else if (PlayerPrefs.GetInt("Id") == 4){
                dbManager.UpdateLevel(5,1);
            }else if (PlayerPrefs.GetInt("Id") == 5){
                dbManager.UpdateLevel(6,1);
            }
            ShowResultPanel();
        }
    }

    // Hàm hiển thị thông báo thất bại
    public void DisplayDefeatMessage()
    {
        if (!gameOver) // Đảm bảo chỉ hiển thị kết quả một lần
        {
            resultImage.sprite = defeatSprite;
            Debug.Log("Defeat message displayed!");
            ShowResultPanel();
        }
    }

    // Hàm hiển thị panel kết quả
    private void ShowResultPanel()
    {
        infoPanel.SetActive(true); // Hiện panel chứa hình ảnh kết quả
        infoPanel.transform.SetAsLastSibling();
        Debug.Log("Result panel is now visible!");
        DatabaseManager.GameData gameData = dbManager.GetGold();
        dbManager.UpdateGold(gameData.NewGoldUpdate);
        dbManager.UpdateGoldRoundCurrent(0);
        Time.timeScale = 0; // Dừng thời gian trò chơi
        gameOver = true; // Đánh dấu kết thúc game
        StartCoroutine(GoToMainSceneAfterDelay());
    }

    public void ShowPanelEnd()
    {
        PanelEnd.SetActive(true); // Hiện panel chứa hình ảnh kết quả
        PanelEnd.transform.SetAsLastSibling();
        Time.timeScale = 0; // Dừng thời gian trò chơi
    }

    public void CloseEndPanel()
    {
        PanelEnd.SetActive(false); // Ẩn PanelEnd
        Time.timeScale = 1; // Khôi phục thời gian trò chơi
    }


    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1; // Khôi phục thời gian trò chơi nếu bị dừng
        DatabaseManager.GameData gameData = dbManager.GetGold();
        dbManager.UpdateGold(gameData.NewGoldUpdate);
        dbManager.UpdateGoldRoundCurrent(0);
        SceneManager.LoadScene(sceneName); // Chuyển sang scene khác
    }
    IEnumerator GoToMainSceneAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f); // Sử dụng thời gian thực thay vì thời gian trò chơi
        ChangeScene("MainScene");
    }
}

