using TMPro;
using Unity.Burst;
using UnityEngine.UI;

namespace Scripts.UI.PlayerPrefs
{
    using UnityEngine;
    [BurstCompile]
    public class PlayerPrefsSetter_Slider : PlayerPrefsSetter<int>
    {
        [SerializeField] private TMP_Text handleText;
        private Slider slider;

        protected override void Start()
        {
            slider = GetComponent<Slider>();

            Value = PlayerPrefs.GetInt(Key, DefaultValue);
            PlayerPrefs.SetInt(Key, Value);

            slider.value = Value;
            slider.onValueChanged.Invoke(Value);
        }
        public void OnValueChanged(float value)
        {
            Value = (int)value;
            handleText.text = Value.ToString();
            PlayerPrefs.SetFloat(Key, Value);
        }
    }
}