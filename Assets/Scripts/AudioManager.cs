using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    public AudioClip Background;
    public AudioClip Battle;

    private bool isMutedBackground = false; 
    private bool isMutedBattle = false; 
    private void Awake()
    {
        // Kiểm tra nếu instance đã tồn tại
        if (Instance == null)
        {
            Instance = this; // Gán instance
            DontDestroyOnLoad(gameObject); // Không phá hủy đối tượng khi chuyển cảnh
        }
        else
        {
            Destroy(gameObject); // Nếu đã có instance, phá hủy đối tượng mới
        }
    }

    private void Start()
    {
        musicSource.clip = Background;
        musicSource.loop = true;  // Đặt nhạc nền lặp lại
        musicSource.Play();        // Phát nhạc nền
    }

    // Hàm để bật/tắt âm thanh
    public void ToggleMute(bool muteBackground, bool muteBattle)
    {
        isMutedBackground = muteBackground;
        isMutedBattle = muteBattle;
        if (isMutedBackground)
        {
            musicSource.mute = true; 
        }else{
            musicSource.mute = false; 
        }
        if (isMutedBattle)
        {
            SFXSource.mute = true;
        }else{
            SFXSource.mute = false;
        }
        
    }
    public void ChangeMusic(AudioClip newClip)
    {
        if (musicSource.clip != newClip)
        {
            musicSource.Stop();        // Dừng nhạc hiện tại
            musicSource.clip = newClip; // Đặt clip mới
            musicSource.Play();        // Phát nhạc mới
        }
    }
    public bool IsMutedBackground()
    {
        return isMutedBackground;
    }

    public bool IsMutedBattle()
    {
        return isMutedBattle;
    }
}
