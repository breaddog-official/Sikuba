using TMPro;
using UnityEngine;

public class PlayerPrefsSetter_InputField : PlayerPrefsSetter<string>
{
    private TMP_InputField inputField;
    private void Start()
    {
        inputField = GetComponent<TMP_InputField>();

        Value = PlayerPrefs.GetString(Key, DefaultValue);
        PlayerPrefs.SetString(Key, Value);

        inputField.text = Value;
    }
    public void OnValueChanged(string value)
    {
        Value = value;
        PlayerPrefs.SetString(Key, Value);
    }
}
