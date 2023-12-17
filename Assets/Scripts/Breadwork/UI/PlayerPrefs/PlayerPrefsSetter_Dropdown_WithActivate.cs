using TMPro;
using UnityEngine;

public class PlayerPrefsSetter_Dropdown_WithActivate : PlayerPrefsSetter<int>
{
    private TMP_Dropdown dropdown;
    private bool isInitialized;
    private void Start()
    {
        if (isInitialized) return;
        else isInitialized = true;

        dropdown = GetComponent<TMP_Dropdown>();

        Value = PlayerPrefs.GetInt(Key, DefaultValue);
        PlayerPrefs.SetInt(Key, Value);

        dropdown.value = Value;
    }
    private void OnEnable()
    {
        if (!isInitialized) Start();
        dropdown.onValueChanged.Invoke(dropdown.value);
    }
    public void OnValueChanged(int value)
    {
        Value = value;
        PlayerPrefs.SetInt(Key, Value);
    }
}
