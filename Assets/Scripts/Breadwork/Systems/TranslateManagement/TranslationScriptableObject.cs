using NaughtyAttributes;
using Scripts.SaveManagement;
using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using System.Net;
using UnityEditor;
using Unity.Burst;
using System.Reflection;
using System.Threading;

namespace Scripts.TranslateManagement
{
#if UNITY_EDITOR
    [BurstCompile]
    [CreateAssetMenu(fileName = "Translation_Language", menuName = "Scripts/Translation", order = 1)]
    public class TranslationScriptableObject : ScriptableObject
    {
        [OnValueChanged(nameof(LoadLanguageWithoutCallback))]
        [JsonIgnore, SerializeField] private ApplicationLanguage currentLanguage = ApplicationLanguage.English;

        public Translation Translation;

        #region Editor
        public void LoadLanguageWithoutCallback()
        {
            try
            {
                Translation = LoadTranslation(currentLanguage);
            }
            catch (FileNotFoundException)
            {
                Translation = new();
            }
        }
        [Button("Load Language")]
        public void LoadLanguageWithCallback()
        {
            try
            {
                Translation = LoadTranslation(currentLanguage);
            }
            catch (FileNotFoundException)
            {
                Debug.Log($"Translation {currentLanguage} not found. This is fine");
                Translation = new();
            }
        }
        public Translation LoadTranslation(ApplicationLanguage language)
        {
            if (SaveManager.ExistsFile(SaveManager.CreatePath
                (Enum.GetName(typeof(ApplicationLanguage), language), TranslateManager.LANGUAGES_SUBFOLDER,
                sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication)))
            {
                return SaveManager.LoadFromFile<Translation>
                (Enum.GetName(typeof(ApplicationLanguage), language),
                subFolder: TranslateManager.LANGUAGES_SUBFOLDER, sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication);
            }
            else
            {
                throw new FileNotFoundException($"{language} not found.");
            }
        }
        public Translation LoadTranslation(string language)
        {
            if (SaveManager.ExistsFile(SaveManager.CreatePath
                (language, TranslateManager.LANGUAGES_SUBFOLDER,
                sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication)))
            {
                return SaveManager.LoadFromFile<Translation>
                (language, subFolder: TranslateManager.LANGUAGES_SUBFOLDER, sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication);
            }
            else
            {
                throw new FileNotFoundException($"{language} not found.");
            }
        }
        [Button]
        public void CreateTranslationFromThis()
        {
            SaveTranslation(ref Translation, currentLanguage);
            Debug.Log("Success!");
            /*AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
            DestroyImmediate(this, true);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();*/
        }
        private void SaveTranslation(ref Translation translation, ApplicationLanguage language)
        {
            SaveManager.SaveToFile(translation, Enum.GetName(typeof(ApplicationLanguage), language),
                SaveManager.Savers.YAML, TranslateManager.LANGUAGES_SUBFOLDER, SaveManager.UpdateSensitivity.UpdateWithApplication);
        }
        [Button("Translate with Google Translater (from English)")]
        public void AutoTranslateWithReflections()
        {
            if (Translation == null)
            {
                Debug.LogWarning("Translation is null!");
                return;
            }

            string code = LanguageCodesConverter.ConvertApplicationLanguageToHLCode(currentLanguage);

            FieldInfo[] translationFields = typeof(Translation).GetFields();
            Translation english = LoadTranslation(ApplicationLanguage.English);
            for (int i = 0; i < translationFields.Length; i++)
            {
                if (translationFields[i].FieldType != typeof(string)) continue;

                string currentValue = (string)translationFields[i].GetValue(Translation);
                string englishValue = (string)translationFields[i].GetValue(english);
                if (string.IsNullOrEmpty(currentValue))
                {
                    GoogleTranslate.TranslateGoogleWithReflections(ref englishValue, Translation, translationFields[i], code);
                }
            }
        }
        public void AutoTranslateWithReflections(ref Translation translation, ApplicationLanguage language)
        {
            if (translation == null)
            {
                Debug.LogWarning("Translation is null!");
                return;
            }

            string code = LanguageCodesConverter.ConvertApplicationLanguageToHLCode(language);

            FieldInfo[] translationFields = typeof(Translation).GetFields();
            Translation english = LoadTranslation(ApplicationLanguage.English);
            for (int i = 0; i < translationFields.Length; i++)
            {
                if (translationFields[i].FieldType != typeof(string)) continue;

                string currentValue = (string)translationFields[i].GetValue(translation);
                string englishValue = (string)translationFields[i].GetValue(english);
                if (string.IsNullOrEmpty(currentValue))
                {
                    GoogleTranslate.TranslateGoogleWithReflections(ref englishValue, translation, translationFields[i], code);
                }
            }
        }
        public void AutoTranslate(ref Translation translation, ApplicationLanguage language)
        {
            if (translation == null)
            {
                Debug.LogWarning("Translation is null!");
                return;
            }
            else if (LanguageCodesConverter.IsGoogleTranslateException(language))
            {
                Debug.Log($"{language} currently is not supported by Google Translate.");
                return;
            }

            string code = LanguageCodesConverter.ConvertApplicationLanguageToHLCode(language);

            FieldInfo[] translationFields = typeof(Translation).GetFields();
            Translation english = LoadTranslation(ApplicationLanguage.English);
            for (int i = 0; i < translationFields.Length; i++)
            {
                if (translationFields[i].FieldType != typeof(string)) continue;

                string currentValue = (string)translationFields[i].GetValue(translation);
                string englishValue = (string)translationFields[i].GetValue(english);
                if (string.IsNullOrEmpty(currentValue))
                {
                    translationFields[i].SetValue(translation, GoogleTranslate.TranslateGoogle(ref englishValue, out bool isDone, code));
                    if (isDone == false)
                    {
                        DebugLogLanguage(language, isDone);
                        return;
                    }
                }
            }
            DebugLogLanguage(language, true);
        }
        private void DebugLogLanguage(ApplicationLanguage language, bool isDone, string reason = null)
        {
            Debug.Log($"<color={(isDone ? nameof(Color.green) : nameof(Color.red))}>" +
                            $"{language} is{(!isDone ? " not" : string.Empty)} translated{(!isDone && reason != null ? $" because {reason}" : "!")}"
                            + "</color>");
        }
        [Button("Translate all languages with Google Translater (from English)")]
        public void TranslateAllLanguages()
        {
            string[] languages = Enum.GetNames(typeof(ApplicationLanguage));
            for (int i = 0; i < languages.Length; i++)
            {
                ApplicationLanguage parsedEnum = (ApplicationLanguage)Enum.Parse(typeof(ApplicationLanguage), languages[i]);
                if (LanguageCodesConverter.IsGoogleTranslateException(parsedEnum))
                {
                    DebugLogLanguage(parsedEnum, false, $"this language currently is not supported by Google Translate.");
                    continue;
                }

                Translation translationFile;
                try
                {
                    translationFile = LoadTranslation(languages[i]);
                }
                catch (FileNotFoundException)
                {
                    //Debug.Log($"Translation {languages[i]} not found. This is fine");
                    translationFile = new();
                }
                AutoTranslate(ref translationFile, parsedEnum);
                SaveTranslation(ref translationFile, parsedEnum);
                Thread.Sleep(50);
            }
        }

        public static class GoogleTranslate
        {
            public static string TranslateGoogle(ref string text, out bool isDone, string translationTo = "en")
            {
                isDone = true;
                var url = string.Format("https://translate.google.com" + "/translate_a/single?client=gtx&dt=t&sl={0}&tl={1}&q={2}",
                    "auto", translationTo, WebUtility.UrlEncode(text));
                UnityWebRequest www = UnityWebRequest.Get(url);
                www.SendWebRequest();
                while (!www.isDone)
                {

                }
                string response = www.downloadHandler.text;

                try
                {
                    JArray jsonArray = JArray.Parse(response);
                    response = jsonArray[0][0][0].ToString();
                }
                catch
                {
                    response = "process error";
                    isDone = false;
                    Debug.LogError("The process is not completed! Most likely, you made too many requests. In this case, the Google Translate API blocks access to the translation for a while.  Please try again later. Do not translate the text too often, so that Google does not consider your actions as spam");
                }

                return response;
            }
            public static void TranslateGoogleWithReflections(ref string text, Translation translation, FieldInfo fieldInfo, string translationTo = "en")
            {
                var url = string.Format("https://translate.google.com" + "/translate_a/single?client=gtx&dt=t&sl={0}&tl={1}&q={2}",
                    "auto", translationTo, WebUtility.UrlEncode(text));
                UnityWebRequest www = UnityWebRequest.Get(url);
                www.SendWebRequest().completed += ctx =>
                {
                    string response = www.downloadHandler.text;

                    try
                    {
                        JArray jsonArray = JArray.Parse(response);
                        response = jsonArray[0][0][0].ToString();
                    }
                    catch
                    {
                        response = "process error";
                        Debug.LogError("The process is not completed! Most likely, you made too many requests. In this case, the Google Translate API blocks access to the translation for a while.  Please try again later. Do not translate the text too often, so that Google does not consider your actions as spam");
                        return;
                    }
                    Debug.Log($"Completed: {fieldInfo.Name}: {response}");
                    fieldInfo.SetValue(translation, response);
                };
            }
        }
        #endregion
    }
#endif
}
