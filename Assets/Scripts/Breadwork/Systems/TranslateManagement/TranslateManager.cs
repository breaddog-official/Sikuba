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
        public const string LANGUAGES_SUBFOLDER = "Languages";

        public static ApplicationLanguage GameLanguage { get; private set; }
        public static event Action GameLanguageChanged;

        public static Translation Translation { get; private set; } = new();

        public static void Initialize() => ChangeLanguage(GetSystemLanguage(), false);
        private static void LoadLanguage()
        {
            Translation = SaveManager.LoadFromFile<Translation>
                (Enum.GetName(typeof(ApplicationLanguage), GameLanguage), SaveManager.Savers.Json, LANGUAGES_SUBFOLDER, SaveManager.UpdateSensitivity.UpdateWithApplication);
        }
        /// <summary>
        /// Returns the system language or, if debugging is enabled, returns English
        /// </summary>
        public static ApplicationLanguage GetSystemLanguage()
        {
            if (GameManager.GameDataConfig.IsDebug)
                return GameManager.GameDataConfig.DefaultLanguage;
            else
                return LanguageCodesConverter.ConvertToApplicationLanguage(Application.systemLanguage);
        }
        /// <summary>
        /// Sets new language
        /// </summary>
        public static void ChangeLanguage(ApplicationLanguage newLanguage, bool withInvoke = true)
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
