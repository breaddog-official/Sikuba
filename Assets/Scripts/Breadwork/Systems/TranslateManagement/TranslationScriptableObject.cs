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
using static UnityEditor.PlayerSettings.Switch;
using System.Collections.Generic;

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
                Translation = TranslateManager.LoadTranslation(currentLanguage);
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
                Translation = TranslateManager.LoadTranslation(currentLanguage);
            }
            catch (FileNotFoundException)
            {
                Debug.Log($"Translation {currentLanguage} not found. This is fine");
                Translation = new();
            }
        }
        [Button]
        public void CreateTranslationFromThis()
        {
            TranslateManager.SaveTranslation(Translation, currentLanguage);
            Debug.Log("Success!");
            /*AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
            DestroyImmediate(this, true);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();*/
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
            Translation english = TranslateManager.LoadTranslation(ApplicationLanguage.English);
            for (int i = 0; i < translationFields.Length; i++)
            {
                if (translationFields[i].FieldType != typeof(string)) continue;

                string currentValue = (string)translationFields[i].GetValue(Translation);
                string englishValue = (string)translationFields[i].GetValue(english);
                if (string.IsNullOrEmpty(currentValue))
                {
                    GoogleTranslate.TranslateGoogleWithReflections(englishValue, Translation, translationFields[i], code);
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
            Translation english = TranslateManager.LoadTranslation(ApplicationLanguage.English);
            for (int i = 0; i < translationFields.Length; i++)
            {
                if (translationFields[i].FieldType != typeof(string)) continue;

                string currentValue = (string)translationFields[i].GetValue(translation);
                string englishValue = (string)translationFields[i].GetValue(english);
                if (string.IsNullOrEmpty(currentValue))
                {
                    GoogleTranslate.TranslateGoogleWithReflections(englishValue, translation, translationFields[i], code);
                }
            }
        }
        public void AutoTranslate(Translation translation, ApplicationLanguage language)
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
            Translation english = TranslateManager.LoadTranslation(ApplicationLanguage.English);

            AsyncGoogleTranslate[] poolTranslaters = new AsyncGoogleTranslate[translationFields.Length];
            ManualResetEvent[] doneEventsPool = new ManualResetEvent[translationFields.Length];

            for (int i = 0; i < translationFields.Length; i++)
            {
                if (translationFields[i].FieldType != typeof(string)) continue;

                string currentValue = (string)translationFields[i].GetValue(translation);
                string englishValue = (string)translationFields[i].GetValue(english);
                if (string.IsNullOrEmpty(currentValue))
                {
                    Debug.Log("asdasd");
                    poolTranslaters[i] = new AsyncGoogleTranslate(englishValue, translation, translationFields[i], doneEventsPool[i], code);
                    doneEventsPool[i] = new ManualResetEvent(false);
                    ThreadPool.QueueUserWorkItem(poolTranslaters[i].StartTranslate);
                    /*translationFields[i].SetValue(translation, GoogleTranslate.TranslateGoogle(englishValue, out bool isDone, code));
                    if (isDone == false)
                    {
                        DebugLogLanguage(language, isDone);
                        return;
                    }*/
                }
            }
            List<ManualResetEvent> resetEvents = new List<ManualResetEvent>();
            for (int i = 0; i < doneEventsPool.Length; i++)
            {
                if (doneEventsPool[i] != null) resetEvents.Add(doneEventsPool[i]);
            }

            WaitHandle.WaitAll(resetEvents.ToArray());
            for (int i = 0; i < poolTranslaters.Length; i++)
            {
                if (poolTranslaters[i].IsDone == false)
                {
                    DebugLogLanguage(language, false);
                    return;
                }
            }
            DebugLogLanguage(language, true);
        }
        public static void DebugLogLanguage(ApplicationLanguage language, bool isDone, string reason = null)
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
                    translationFile = TranslateManager.LoadTranslation(languages[i]);
                }
                catch (FileNotFoundException)
                {
                    //Debug.Log($"Translation {languages[i]} not found. This is fine");
                    translationFile = new();
                }
                AutoTranslate(translationFile, parsedEnum);
                TranslateManager.SaveTranslation(translationFile, parsedEnum);
                Thread.Sleep(50);
            }
        }
        public class AsyncGoogleTranslate
        {
            string text;
            Translation translation;
            FieldInfo fieldInfo;
            ManualResetEvent doneEvent;
            string translationTo = "en";

            public bool IsDone { get; private set; }

            const int maxNumberOfRetry = 32;

            public AsyncGoogleTranslate(string text, Translation translation, FieldInfo fieldInfo, ManualResetEvent doneEvent, string translationTo)
            {
                this.text = text;
                this.translation = translation;
                this.fieldInfo = fieldInfo;
                this.doneEvent = doneEvent;
                this.translationTo = translationTo;
            }
            public void StartTranslate(object context)
            {
                int numberOfRetry = 0;
                do
                {
                    numberOfRetry++;
                    fieldInfo.SetValue(translation, GoogleTranslate.TranslateGoogle(text, out bool isDone, translationTo));
                    IsDone = isDone;
                }
                while (IsDone == false || numberOfRetry < maxNumberOfRetry);
                Debug.Log($"перевёл {text} с успехом {IsDone}");
                doneEvent.Set();
            }
        }
        public static class GoogleTranslate
        {
            public static string TranslateGoogle(string text, out bool isDone, string translationTo = "en")
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
            public static void TranslateGoogleWithReflections(string text, Translation translation, FieldInfo fieldInfo, string translationTo = "en")
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
                    fieldInfo.SetValue(translation, response);
                };
            }
        }
        #endregion
    }
#endif
}
