using UnityEngine;

namespace Scripts.Settings
{
    public abstract class SettingUI : MonoBehaviour
    {
        [field: SerializeField] public int Index { get; protected set; }

        protected void OnChangedValue(object value)
        {
            SettingsManager.Settings[Index].ChangeSetting(value);
            SettingsManager.Save();
        }
    }
}
