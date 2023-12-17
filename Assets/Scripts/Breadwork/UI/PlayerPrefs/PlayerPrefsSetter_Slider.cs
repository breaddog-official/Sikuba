using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsSetter_Slider : PlayerPrefsSetter<int>
{
    [SerializeField] private TMP_Text handleText;
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();

        Value = PlayerPrefs.GetInt(Key, DefaultValue);
        PlayerPrefs.SetInt(Key, Value);

        slider.value = Value;
    }
    public void OnValueChanged(float value)
    {
        Value = (int)value;
        handleText.text = Value.ToString();
        PlayerPrefs.SetFloat(Key, Value);
    }
}
