using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using Scripts.Settings;

namespace Scripts.UI
{
    public class ResolutionDropdownHandler : MonoBehaviour
    {
        private void Start()
        {
            TMP_Dropdown resolutionDropdown = GetComponent<TMP_Dropdown>();
            Resolution[] resolutions = Screen.resolutions;
            List<string> resolutionNames = new List<string>();
            for (int i = 0; i < resolutions.Length; i++)
            {
                resolutionNames.Add($"{resolutions[i].width}x{resolutions[i].height} {Math.Floor(resolutions[i].refreshRateRatio.value)}hz");
            }
            resolutionDropdown.AddOptions(resolutionNames);
            resolutionDropdown.value = SettingsManager.ReadValue<int>(2);
        }
    }
}
