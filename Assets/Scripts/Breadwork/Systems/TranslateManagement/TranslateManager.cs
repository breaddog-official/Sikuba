using Scripts.Core;
using Scripts.SaveManagement;
using System;
using Unity.Burst;
using UnityEngine;
using YamlDotNet.Core.Tokens;

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
        #region Load & Save
        public static void SaveTranslation(Translation Translation, ApplicationLanguage Language)
        {
            SaveManager.SaveToFile(Translation, Enum.GetName(typeof(ApplicationLanguage), Language),
                SaveManager.Savers.YAML, LANGUAGES_SUBFOLDER, SaveManager.UpdateSensitivity.UpdateWithApplication);
        }
        public static Translation LoadTranslation(ApplicationLanguage language)
        {
            if (SaveManager.ExistsFile(SaveManager.CreatePath
                (Enum.GetName(typeof(ApplicationLanguage), language), SaveManager.Savers.YAML, LANGUAGES_SUBFOLDER,
                sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication)))
            {
                return SaveManager.LoadFromFile<Translation>
                (Enum.GetName(typeof(ApplicationLanguage), language), SaveManager.Savers.YAML,
                subFolder: LANGUAGES_SUBFOLDER, sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication);
            }
            else
            {
                throw new FileNotFoundException($"{language} not found.");
            }
        }
        public static Translation LoadTranslation(string language)
        {
            if (SaveManager.ExistsFile(SaveManager.CreatePath
                (language, SaveManager.Savers.YAML, LANGUAGES_SUBFOLDER,
                sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication)))
            {
                return SaveManager.LoadFromFile<Translation>
                (language, SaveManager.Savers.YAML, subFolder: LANGUAGES_SUBFOLDER, sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication);
            }
            else
            {
                throw new FileNotFoundException($"{language} not found.");
            }
        }
        #endregion
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
                Translation = LoadTranslation(GameLanguage);
            }
            if (withInvoke) GameLanguageChanged?.Invoke();
        }
        /// <summary>
        /// Sets new language
        /// </summary>
        public static void ChangeLanguageString(string newLanguage, bool withInvoke = true)
        {
            if (Enum.TryParse(newLanguage, out ApplicationLanguage languageValue))
            {
                ChangeLanguage(languageValue, withInvoke);
            }
            else
            {
                try
                {
                    ChangeLanguageCustom(SaveManager.LoadFromPath<Translation>
                    (SaveManager.CreatePath(newLanguage, SaveManager.Savers.YAML, LANGUAGES_SUBFOLDER, SaveManager.UpdateSensitivity.UpdateWithApplication), SaveManager.Savers.YAML),
                    withInvoke);
                }
                catch (FileNotFoundException)
                {
                    ErrorMessageFabric.CreateInstance("Language not found", 
                        "The file for this language was not found. We have no idea why this could have happened. You may have deleted this file after starting the game.");
                }
            }
            if (withInvoke && GameLanguageChanged != null) GameLanguageChanged.Invoke();
        }
        /// <summary>
        /// Sets new custom language
        /// </summary>
        public static void ChangeLanguageCustom(Translation newTranslation, bool withInvoke = true)
        {
            GameLanguage = ApplicationLanguage.Unknown;
            Translation = newTranslation;

            if (withInvoke && GameLanguageChanged != null) GameLanguageChanged.Invoke();
        }
    }
}
