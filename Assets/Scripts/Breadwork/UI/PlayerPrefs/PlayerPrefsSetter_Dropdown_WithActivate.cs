using TMPro;

namespace Scripts.UI.PlayerPrefs
{
    using Unity.Burst;
    using UnityEngine;
    [BurstCompile]
    public class PlayerPrefsSetter_Dropdown_WithActivate : PlayerPrefsSetter<int>
    {
        private TMP_Dropdown dropdown;
        private bool isInitialized;
        protected override void Start()
        {
            if (isInitialized) return;
            else isInitialized = true;

            dropdown = GetComponent<TMP_Dropdown>();

            Value = PlayerPrefs.GetInt(Key, DefaultValue);
            PlayerPrefs.SetInt(Key, Value);

            dropdown.value = Value;
            dropdown.onValueChanged.Invoke(Value);
        }
        protected override void OnEnable()
        {
            if (!isInitialized) Start();
            dropdown.onValueChanged.Invoke(dropdown.value);
            base.OnEnable();
        }
        public void OnValueChanged(int value)
        {
            Value = value;
            PlayerPrefs.SetInt(Key, Value);
        }
    }
}
