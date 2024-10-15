using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagement : MonoBehaviour
{
    public Image image1; // Kéo thả Image vào đây từ Inspector
    public Image image2; // Kéo thả Image vào đây từ Inspector
    public Image image3;
    public Image imageTimeline1; // Kéo thả Image vào đây từ Inspector
    public Image imageTimeline2; // Kéo thả Image vào đây từ Inspector
    public Image imageTimeline3;
    private DatabaseManager databaseManager;
    private BattleScript battleScript;
    private Uprage uprage;
    private Color originalColor;
    void Awake(){
        databaseManager = FindObjectOfType<DatabaseManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        battleScript = FindObjectOfType<BattleScript>();
        uprage = FindObjectOfType<Uprage>();
        originalColor = Color.white; 
        Color grayColor = new Color(94f / 255f, 94f / 255f, 94f / 255f, 1f);
        var levels = databaseManager.GetAllLevels();
    
    // Duyệt qua từng bản ghi trong bảng Level
        for (int i = levels.Count - 1; i >=0 ; i--)
        {
            DatabaseManager.Level level = levels[i]; // Lấy từng đối tượng Level
            if ((level.Id == 3) &&(level.Pass == 1)){
                image3.gameObject.SetActive(false);
                image2.gameObject.SetActive(false);
                image1.gameObject.SetActive(false);
                imageTimeline3.color =  grayColor;
                return;
            }
            if ((level.Id == 2) && (level.Pass == 1)){
                image2.gameObject.SetActive(false);
                image1.gameObject.SetActive(false);
                imageTimeline2.color =  grayColor;
                return;
            }
            if ((level.Id == 1) && (level.Pass == 1)){
                image1.gameObject.SetActive(false);
                imageTimeline1.color =  grayColor;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void changeLevel(int id){
        originalColor = Color.white; 
        Color grayColor = new Color(94f / 255f, 94f / 255f, 94f / 255f, 1f);
        var levels = databaseManager.GetAllLevels();
        battleScript.Id = id;
        uprage.id = id;
    
    // Duyệt qua từng bản ghi trong bảng Level
        for (int i = levels.Count - 1; i >=0 ; i--)
        {
            DatabaseManager.Level level = levels[i]; // Lấy từng đối tượng Level
            if ((level.Id == 3) && (level.Pass == 1)){
                image3.gameObject.SetActive(false);
                image2.gameObject.SetActive(false);
                image1.gameObject.SetActive(false);
                if (id == 3){
                    imageTimeline3.color =  grayColor;
                    imageTimeline2.color =  originalColor;
                    imageTimeline1.color =  originalColor;
                }else if(id == 2){
                    imageTimeline2.color =  grayColor;
                    imageTimeline3.color =  originalColor;
                    imageTimeline1.color =  originalColor;
                }else{
                    imageTimeline1.color =  grayColor;
                    imageTimeline3.color =  originalColor;
                    imageTimeline2.color =  originalColor;
                }
            }
            if ((level.Id == 2) && (level.Pass == 1)){
                image2.gameObject.SetActive(false);
                image1.gameObject.SetActive(false);
                if(id == 2){
                    imageTimeline2.color =  grayColor;
                    imageTimeline3.color =  originalColor;
                    imageTimeline1.color =  originalColor;
                }else{
                    imageTimeline1.color =  grayColor;
                    imageTimeline3.color =  originalColor;
                    imageTimeline2.color =  originalColor;
                }
            }
        }
    }
}
