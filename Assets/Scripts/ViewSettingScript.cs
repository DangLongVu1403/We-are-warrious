using UnityEngine;
using UnityEngine.UI;



public class ViewSettingScript : MonoBehaviour
{

    public GameObject settingSoundPanel;
    public GameObject settingLanguagePanel;
    private AudioManager audioManager;
    private bool isMutedBackground; 
    private bool isMutedBattle; 
    public Button muteBattleButton;
    public Button muteBackgroundButton;
    public Sprite muteSprite;  // Hình ảnh khi âm thanh bị tắt
    public Sprite unmuteSprite; // Hình ảnh khi âm thanh được bật
    void Awake(){
        settingSoundPanel.SetActive(false);
        settingLanguagePanel.SetActive(false);
        audioManager = AudioManager.Instance;
    }
   // Xử lý nút ngôn ngữ
    public void ChangeLanguage()
    {
       settingLanguagePanel.SetActive(true);
    }

    public void SettingSound(){
        settingSoundPanel.SetActive(true);
    }

    public void closePanelSettingSound(){
        settingSoundPanel.SetActive(false);
    }

    public void closePanelSettingLanguage(){
        settingLanguagePanel.SetActive(false);
    }

    public void muteSoundBackground(){
        if(audioManager.IsMutedBackground()){
            audioManager.ToggleMute(false,audioManager.IsMutedBattle());
            ChangeButtonBackgroundImage(false);
        }else{
            audioManager.ToggleMute(true,audioManager.IsMutedBattle());
            ChangeButtonBackgroundImage(true);
        }
    }

    public void muteSoundBattle(){
        if(audioManager.IsMutedBattle()){
            audioManager.ToggleMute(audioManager.IsMutedBackground(),false);
            ChangeButtonBattleImage(false);
        }else{
            audioManager.ToggleMute(audioManager.IsMutedBackground(),true);
            ChangeButtonBattleImage(true);
        }
    }

    private void ChangeButtonBackgroundImage(bool muteBackground)
    {
        if (muteBackground)
        {
            muteBackgroundButton.image.sprite = muteSprite; // Đổi ảnh khi nhạc nền tắt
        }
        else
        {
            muteBackgroundButton.image.sprite = unmuteSprite; // Đổi ảnh khi nhạc nền bật
        }
    }

    private void ChangeButtonBattleImage(bool muteBattle)
    {
        if (muteBattle)
        {
            muteBattleButton.image.sprite = muteSprite; // Đổi ảnh khi nhạc nền tắt
        }
        else
        {
            muteBattleButton.image.sprite = unmuteSprite; // Đổi ảnh khi nhạc nền bật
        }
    }
    // Xử lý nút thoát game
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
