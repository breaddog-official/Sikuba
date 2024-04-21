using Unity.Burst;
using UnityEngine;

namespace Scripts.Settings
{
    [BurstCompile]
    public abstract class SettingUI : MonoBehaviour
    {
        [field: SerializeField] public int Index { get; protected set; }
        private bool firstSet = true;

        protected void OnChangedValue(object value)
        {
            if (firstSet)
            {
                firstSet = false;
                return;
            }
            SettingsManager.Settings[Index].ChangeSetting(value);
            SettingsManager.Save();
        }
    }
}
