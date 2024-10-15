using UnityEngine;
using TMPro;  // Thư viện cho TMP_Dropdown
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using System.Collections;

public class LanguageSwitcher : MonoBehaviour
{
    public TMP_Dropdown languageDropdown; // Dropdown chứa các ngôn ngữ
    public GameObject settingLanguagePanel;
    public Button confirmButton;          // Nút "Xác nhận"

    private void Start()
    {
        // Lắng nghe sự kiện nhấn nút "Xác nhận"
        confirmButton.onClick.AddListener(OnConfirmButtonClicked);
    }

    private void OnConfirmButtonClicked()
    {
        // Lấy chỉ số của option được chọn trong Dropdown
        int selectedOption = languageDropdown.value;

        // Tùy thuộc vào chỉ số option, ta sẽ đổi ngôn ngữ
        if (selectedOption == 0) // Nếu chọn "Tiếng Việt"
        {
            StartCoroutine(SetLocale("vi-VN"));  // Locale ID cho Tiếng Việt
        }
        else if (selectedOption == 1) // Nếu chọn "English"
        {
            StartCoroutine(SetLocale("en"));  // Locale ID cho English
        }
        settingLanguagePanel.SetActive(false);
    }

    // Coroutine để thay đổi ngôn ngữ
    IEnumerator SetLocale(string localeCode)
    {
        yield return LocalizationSettings.InitializationOperation;  // Đợi hoàn tất khởi tạo Localization

        // Lấy danh sách các Locale có sẵn
        var availableLocales = LocalizationSettings.AvailableLocales.Locales;

        // Tìm locale dựa trên mã (localeCode)
        foreach (var locale in availableLocales)
        {
            if (locale.Identifier.Code == localeCode)
            {
                LocalizationSettings.SelectedLocale = locale;  // Thay đổi Locale
                break;
            }
        }
    }
}
