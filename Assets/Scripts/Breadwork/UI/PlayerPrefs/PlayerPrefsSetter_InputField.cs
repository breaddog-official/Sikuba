using TMPro;
using Unity.Burst;

namespace Scripts.UI.PlayerPrefs
{
    using UnityEngine;

    [BurstCompile]
    public class PlayerPrefsSetter_InputField : PlayerPrefsSetter<string>
    {
        private TMP_InputField inputField;
        protected override void Start()
        {
            inputField = GetComponent<TMP_InputField>();

            Value = PlayerPrefs.GetString(Key, DefaultValue);
            PlayerPrefs.SetString(Key, Value);

            inputField.text = Value;
            inputField.onValueChanged.Invoke(Value);
        }
        public void OnValueChanged(string value)
        {
            Value = value;
            PlayerPrefs.SetString(Key, Value);
        }
    }
}
