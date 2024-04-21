using TMPro;

namespace Scripts.UI.PlayerPrefs
{
    using Unity.Burst;
    using UnityEngine;
    [BurstCompile]
    public class PlayerPrefsSetter_Dropdown : PlayerPrefsSetter<int>
    {
        private TMP_Dropdown dropdown;
        protected override void Start()
        {
            dropdown = GetComponent<TMP_Dropdown>();

            Value = PlayerPrefs.GetInt(Key, DefaultValue);
            PlayerPrefs.SetInt(Key, Value);

            dropdown.value = Value;
            dropdown.onValueChanged.Invoke(Value);
        }
        public void OnValueChanged(int value)
        {
            Value = value;
            PlayerPrefs.SetInt(Key, Value);
        }
    }
}
