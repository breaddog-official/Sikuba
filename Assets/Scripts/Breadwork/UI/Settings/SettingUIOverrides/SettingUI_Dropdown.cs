using TMPro;
using Unity.Burst;

namespace Scripts.Settings
{
    [BurstCompile]
    public class SettingUI_Dropdown : SettingUI
    {
        TMP_Dropdown dropdown;
        protected virtual void Start()
        { 
            dropdown = GetComponent<TMP_Dropdown>();
            dropdown.value = SettingsManager.ReadValue<int>(Index);
            dropdown.onValueChanged.Invoke(dropdown.value);
        }
        public void ChangeSettingInt(int value)
        {
            OnChangedValue(value);
        }
    }
}
