using Scripts.Core;
using Scripts.SaveManagement;
using System;
using Unity.Burst;
using UnityEngine;

namespace Scripts.TranslateManagement
{
    [BurstCompile]
    public static class TranslateManager
    {
        public const string LANGUAGES_SUBFOLDER = "Resources/Languages/";

        public static SystemLanguage GameLanguage { get; private set; }
        public static event Action GameLanguageChanged;

        public static string[] Translation { get; private set; }

        public static void Initialize() => ChangeLanguage(GetSystemLanguage(), false);
        private static void LoadLanguage()
        {
            Translation = SaveManager.LoadFromFile<string[]>
                (Enum.GetName(typeof(SystemLanguage), GameLanguage), SaveManager.Savers.YAML, LANGUAGES_SUBFOLDER);
        }
        /// <summary>
        /// Returns the system language or, if debugging is enabled, returns English
        /// </summary>
        public static SystemLanguage GetSystemLanguage()
        {
            if (GameManager.GameDataConfig.IsDebug)
                return SystemLanguage.English;
            else
                return Application.systemLanguage;
        }
        /// <summary>
        /// Sets new language
        /// </summary>
        public static void ChangeLanguage(SystemLanguage newLanguage, bool withInvoke = true)
        {
            if (GameLanguage != newLanguage)
            {
                GameLanguage = newLanguage;
                LoadLanguage();
            }
            GameLanguageChanged?.Invoke();
        }
    }
}
