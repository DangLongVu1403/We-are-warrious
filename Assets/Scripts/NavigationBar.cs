using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Thêm UnityEngine.UI để thao tác với Button

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject ViewUprage;
    public GameObject ViewBattle;
    public GameObject ViewLevel;
    public GameObject ViewSetting;

    // Thêm tham chiếu đến các Button
    public Button buttonUprage;
    public Button buttonBattle;
    public Button buttonLevel;
    public Button buttonSetting;

    public Color selectedColor = Color.blue;   // Màu khi Button được chọn
    public Color normalColor = Color.white;    // Màu mặc định của Button

    void Start()
    {
        ShowPanelBattle();  // Hiển thị panel Battle khi khởi động
    }

    // Hàm này để đặt lại màu cho tất cả các Button
    private void ResetButtonColors()
    {
        buttonUprage.GetComponent<Image>().color = normalColor;
        buttonBattle.GetComponent<Image>().color = normalColor;
        buttonLevel.GetComponent<Image>().color = normalColor;
        buttonSetting.GetComponent<Image>().color = normalColor;
    }

    public void ShowPanelUprage()
    {
        ViewUprage.SetActive(true);
        ViewBattle.SetActive(false);
        ViewLevel.SetActive(false);
        ViewSetting.SetActive(false);

        // Đổi màu Button
        ResetButtonColors();
        buttonUprage.GetComponent<Image>().color = selectedColor;
    }

    public void ShowPanelBattle()
    {
        ViewUprage.SetActive(false);
        ViewBattle.SetActive(true);
        ViewLevel.SetActive(false);
        ViewSetting.SetActive(false);

        // Đổi màu Button
        ResetButtonColors();
        buttonBattle.GetComponent<Image>().color = selectedColor;
    }

    public void ShowPanelLevel()
    {
        ViewUprage.SetActive(false);
        ViewBattle.SetActive(false);
        ViewLevel.SetActive(true);
        ViewSetting.SetActive(false);

        // Đổi màu Button
        ResetButtonColors();
        buttonLevel.GetComponent<Image>().color = selectedColor;
    }

    public void ShowPanelUSetting()
    {
        ViewUprage.SetActive(false);
        ViewBattle.SetActive(false);
        ViewLevel.SetActive(false);
        ViewSetting.SetActive(true);

        // Đổi màu Button
        ResetButtonColors();
        buttonSetting.GetComponent<Image>().color = selectedColor;
    }
}
