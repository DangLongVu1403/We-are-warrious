using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;  

public class LanguageDropdownHandler : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;
    public LocalizedString option1; // Localized cho option 1
    public LocalizedString option2; // Localized cho option 2

    private void Start()
    {
        // Lắng nghe sự thay đổi ngôn ngữ
        LocalizationSettings.SelectedLocaleChanged += UpdateDropdownOptions;

        // Cập nhật các option trong dropdown theo ngôn ngữ hiện tại
        UpdateDropdownOptions(LocalizationSettings.SelectedLocale);
    }

    // Hàm cập nhật các option trong Dropdown
    private void UpdateDropdownOptions(UnityEngine.Localization.Locale locale)
    {
        // Xóa các option cũ
        languageDropdown.options.Clear();

        // Lấy văn bản đã dịch cho từng option
        string localizedOption1 = option1.GetLocalizedString();
        string localizedOption2 = option2.GetLocalizedString();

        // Thêm các option đã dịch vào Dropdown
        languageDropdown.options.Add(new TMP_Dropdown.OptionData(localizedOption1));
        languageDropdown.options.Add(new TMP_Dropdown.OptionData(localizedOption2));

        // Làm mới Dropdown để hiển thị đúng
        languageDropdown.RefreshShownValue();
    }

    private void OnDestroy()
    {
        // Hủy sự kiện khi đối tượng bị hủy
        LocalizationSettings.SelectedLocaleChanged -= UpdateDropdownOptions;
    }
}
