using TMPro;
using UnityEngine;

public class PlayerPrefsSetter_Dropdown : PlayerPrefsSetter<int>
{
    private TMP_Dropdown dropdown;
    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();

        Value = PlayerPrefs.GetInt(Key, DefaultValue);
        PlayerPrefs.SetInt(Key, Value);

        dropdown.value = Value;
    }
    public void OnValueChanged(int value)
    {
        Value = value;
        PlayerPrefs.SetInt(Key, Value);
    }
}
