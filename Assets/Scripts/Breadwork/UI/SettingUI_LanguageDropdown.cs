using System.Collections.Generic;
using TMPro;
using Scripts.Settings;
using Unity.Burst;
using Scripts.TranslateManagement;
using System.IO;
using Scripts.SaveManagement;

namespace Scripts.UI
{
    [BurstCompile]
    public class SettingUI_LanguageDropdown : SettingUI
    {
        TMP_Dropdown languageDropdown;
        private void Start()
        {
            languageDropdown = GetComponent<TMP_Dropdown>();

            string[] languagesFolderFiles = Directory.GetFiles(SaveManager.CreatePath(TranslateManager.LANGUAGES_SUBFOLDER, SaveManager.UpdateSensitivity.UpdateWithApplication));
            List<string> languagesNames = new List<string>();
            for (int i = 0; i < languagesFolderFiles.Length; i++)
            {
                if (Path.GetExtension(languagesFolderFiles[i]) != ".meta")
                {
                    languagesNames.Add(Path.GetFileNameWithoutExtension(languagesFolderFiles[i]));
                }
            }

            languageDropdown.ClearOptions();
            languageDropdown.AddOptions(languagesNames);

            string currentLanguageName = SettingsManager.ReadValue<string>(5);
            for (int i = 0; i < languagesNames.Count; i++)
            {
                if (string.Equals(languagesNames[i], currentLanguageName))
                {
                    languageDropdown.value = i;
                }
            }
        }
        public void ChangeSettingLanguage(int value)
        {
            OnChangedValue(languageDropdown.options[value].text);
        }
    }
}

