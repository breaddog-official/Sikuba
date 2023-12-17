using TMPro;

namespace Scripts.Settings
{
    public class SettingUI_Dropdown : SettingUI
    {
        private void Start()
        {
            GetComponent<TMP_Dropdown>().value = SettingsManager.ReadValue<int>(Index);
        }
        public void ChangeSettingInt(int value)
        {
            OnChangedValue(value);
        }
    }
}
