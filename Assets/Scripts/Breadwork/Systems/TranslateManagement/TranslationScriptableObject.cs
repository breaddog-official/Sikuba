using NaughtyAttributes;
using Scripts.SaveManagement;
using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using UnityEngine.Networking;
using System.Net;
using UnityEditor;
using Unity.Burst;
using System.Reflection;
using System.Threading.Tasks;
using Mono.CecilX.Cil;
using YamlDotNet.Core.Tokens;

namespace Scripts.TranslateManagement
{
    [BurstCompile, Serializable]
    public class Translation
    {
        [JsonProperty, ResizableTextArea, BoxGroup("Menu")] public string menu_play;
        [JsonProperty, ResizableTextArea, BoxGroup("Menu")] public string menu_options;
        [JsonProperty, ResizableTextArea, BoxGroup("Menu")] public string menu_quit;
    }
#if UNITY_EDITOR
    [BurstCompile]
    [CreateAssetMenu(fileName = "Translation_Language", menuName = "Scripts/Translation", order = 1)]
    public class TranslationScriptableObject : ScriptableObject
    {

        [OnValueChanged(nameof(LoadLanguageWithoutCallback))]
        [JsonIgnore, SerializeField] private SystemLanguage currentLanguage = SystemLanguage.English;

        public Translation Translation;

        #region Editor
        private void Awake() => LoadLanguageWithoutCallback();

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
        //[Button("Load Language")]
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
        public Translation LoadTranslation(SystemLanguage language)
        {
            if (SaveManager.ExistsFile(SaveManager.CreatePath
                (Enum.GetName(typeof(SystemLanguage), language), TranslateManager.LANGUAGES_SUBFOLDER,
                sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication)))
            {
                return SaveManager.LoadFromFile<Translation>
                (Enum.GetName(typeof(SystemLanguage), language),
                subFolder: TranslateManager.LANGUAGES_SUBFOLDER, sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication);
            }
            else
            {
                throw new FileNotFoundException($"{language} not found.");
            }
        }
        [Button]
        public void CreateTranslationFromThis()
        {
            SaveManager.SaveToFile(Translation, Enum.GetName(typeof(SystemLanguage), currentLanguage),
                SaveManager.Savers.YAML, TranslateManager.LANGUAGES_SUBFOLDER, SaveManager.UpdateSensitivity.UpdateWithApplication);

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
            DestroyImmediate(this, true);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        [Button("Translate with Google Translater (from English)")]
        public void AutoTranslate()
        {
            if (Translation == null)
            {
                Debug.LogWarning("Translation is null!");
                return;
            }
            string code = LanguageCodesConverter.ConvertToCode(currentLanguage);


            FieldInfo[] translationFields = typeof(Translation).GetFields();
            Translation english = LoadTranslation(SystemLanguage.English);
            for (int i = 0; i < translationFields.Length; i++)
            {
                if (translationFields[i].FieldType != typeof(string)) continue;

                string currentValue = (string)translationFields[i].GetValue(Translation);
                string englishValue = (string)translationFields[i].GetValue(english);
                if (string.IsNullOrEmpty(currentValue))
                {
                    TranslateGoogleWithReflections(englishValue, translationFields[i], code);
                }
            }
        }
        private string TranslateGoogle(ref string text, string translationTo = "en")
        {
            var url = String.Format("https://translate.google.com" + "/translate_a/single?client=gtx&dt=t&sl={0}&tl={1}&q={2}",
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
                Debug.LogError("The process is not completed! Most likely, you made too many requests. In this case, the Google Translate API blocks access to the translation for a while.  Please try again later. Do not translate the text too often, so that Google does not consider your actions as spam");
            }

            return response;
        }
        private void TranslateGoogleWithReflections(string text, FieldInfo fieldInfo, string translationTo = "en")
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
                fieldInfo.SetValue(Translation, response);
            };
        }
        #endregion
    }
#endif
}
