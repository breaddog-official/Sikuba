using UnityEngine;
using NaughtyAttributes;
using Unity.Burst;
using System;
using Scripts.Core;
using System.Collections.Generic;
using Scripts.SaveManagement;
using Scripts.TranslateManagement;

namespace Scripts.Settings
{
    [BurstCompile]
    public class SettingsManager : MonoBehaviour
    {
        //[SerializeField, BoxGroup("Scene Links")] private UniversalAdditionalCameraData[] cameras;
        //[SerializeField, BoxGroup("Scene Links")] private Volume[] volumes;
        //[SerializeField, BoxGroup("Scene Links")] private VolumeProfile[] volumeProfiles;
        //[SerializeField, BoxGroup("Scene Links")] private ReflectionProbe[] reflectionProbes;

        public static SettingsManager Instance { get; private set; }
        public static List<ISetting> Settings { get; set; }

        private const string SAVE_NAME = "settings";

        private void Awake() => Instance = this;
        private void Start() => Apply();
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void Initialize()
        {
            Settings = new();
            InitializeSettings();
            Load();
            Save();
        }
        /// <summary>
        /// Saves settings in file
        /// </summary>
        public static void Save()
        {
            List<SettingToSave> listToSave = new();
            foreach (ISetting s in Settings)
            {
                listToSave.Add(new SettingToSave(s.Name, s.Value));
            }
            SaveManager.SaveToFile(listToSave, SAVE_NAME);
        }
        private static void Load()
        {
            List<SettingToSave> loadedList;
            try
            {
                loadedList = SaveManager.LoadFromFile<List<SettingToSave>>(SAVE_NAME);
            }
            catch (FileNotFoundException)
            {
                Debug.Log("Settings not found. Creating...");
                return;
            }
            for (int i = 0; i < Settings.Count; i++)
            {
                Settings[i].ChangeSetting(loadedList[i].Value, false);
            }
        }
        /// <summary>
        /// Applies settings
        /// </summary>
        public static void Apply()
        {
            foreach (ISetting s in Settings)
            {
                s.Invoke();
            }
        }
        public static T ReadValue<T>(int settingIndex)
        {
            return GameManager.AdvancedConvert<T>(Settings[settingIndex].Value);
        }
        /// <summary>
        /// Not recommended because this is a costly function
        /// </summary>
        public static T ReadValue<T>(string settingName)
        {
            foreach(ISetting s in Settings)
            {
                if (s.Name == settingName)
                {
                    return GameManager.AdvancedConvert<T>(s.Value);
                }
            }
            throw new Exception($"Setting with '{settingName}' name do not exist. Check {nameof(SettingsManager)} script.");
            
        }
        #region names
        // MusicVolume = 0
        // SoundVolume = 1
        // ResolutionIndex = 2
        // Fullscreen = 3
        // RefreshRate = 4
        // Language = 5
        #endregion
        private static void InitializeSettings()
        {
            Settings.Add(new Setting<float>(defaultValue: 0.8f)
            {
                name = "MusicVolume",
                action = (float value) =>
                {
                    GameManager.GameDataConfig.MusicMixer.audioMixer.SetFloat("MusicMaster", Mathf.Log10(value) * 20);
                },
            });
            Settings.Add(new Setting<float>(defaultValue: 0.8f)
            {
                name = "SoundVolume",
                action = (float value) =>
                {
                    GameManager.GameDataConfig.SoundMixer.audioMixer.SetFloat("SoundMaster", Mathf.Log10(value) * 20);
                },
            });
            Settings.Add(new Setting<int>(defaultValue: GameManager.GetElementIndexInArray(Screen.currentResolution, Screen.resolutions))
            {
                name = "ResolutionIndex",
                action = (int value) =>
                {
                    if (value == -1) throw new Exception("Resolution is -1");
                    
                    Resolution res = Screen.resolutions[value];
                    Screen.SetResolution(res.width, res.height, Screen.fullScreenMode, res.refreshRateRatio);
                },
            });
            Settings.Add(new Setting<int>(defaultValue: (int)Screen.fullScreenMode)
            {
                name = "Fullscreen",
                action = (int value) =>
                {
                    Screen.fullScreenMode = (FullScreenMode)value;
                },
            });
            Settings.Add(new Setting<int>(defaultValue: (int)RefreshRate.None)
            {
                name = "RefreshRate",
                action = (int value) =>
                {
                    if ((RefreshRate)value == RefreshRate.ScreenHz) Application.targetFrameRate = (int)Math.Floor(Screen.currentResolution.refreshRateRatio.value);
                    else if ((RefreshRate)value == RefreshRate.r30) Application.targetFrameRate = 30;
                    else if ((RefreshRate)value == RefreshRate.r45) Application.targetFrameRate = 45;
                    else if ((RefreshRate)value == RefreshRate.r60) Application.targetFrameRate = 60;
                    else if ((RefreshRate)value == RefreshRate.r120) Application.targetFrameRate = 120;
                    else Application.targetFrameRate = -1;
                },
            });
            Settings.Add(new Setting<int>(defaultValue: (int)TranslateManager.GameLanguage)
            {
                name = "Language",
                action = (int value) =>
                {
                    TranslateManager.ChangeLanguage((SystemLanguage)value);
                },
            });
        }
        
    }
}
