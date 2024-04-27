using NaughtyAttributes;
using Scripts.SaveManagement;
using System;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;
using Newtonsoft.Json.Serialization;
using Unity.Burst;

namespace Scripts.TranslateManagement
{
    [BurstCompile, Serializable]
    public class Translation
    {
        [JsonProperty, ResizableTextArea, BoxGroup("Menu")] public string menu_play;
        [JsonProperty, ResizableTextArea, BoxGroup("Menu")] public string menu_options;
        [JsonProperty, ResizableTextArea, BoxGroup("Menu")] public string menu_quit;
    }
    [BurstCompile]
    [CreateAssetMenu(fileName = "Translation_Language", menuName = "Scripts/Translation", order = 1)]
    public class TranslationScriptableObject : ScriptableObject
    {
        [JsonIgnore, SerializeField, OnValueChanged(nameof(LoadLanguageWithoutCallback))]
        private SystemLanguage currentLanguage = SystemLanguage.English;

        public Translation Translation;

        #region Editor
#if UNITY_EDITOR
        
        private void Awake() => LoadLanguageWithoutCallback();
        
        public void LoadLanguageWithoutCallback()
        {
            if (SaveManager.ExistsFile(SaveManager.CreatePath
                (Enum.GetName(typeof(SystemLanguage), currentLanguage), TranslateManager.LANGUAGES_SUBFOLDER,
                sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication)))
            {
                Translation = SaveManager.LoadFromFile<Translation>
                (Enum.GetName(typeof(SystemLanguage), currentLanguage),
                subFolder: TranslateManager.LANGUAGES_SUBFOLDER, sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication);
            }
            else
            {
                Translation = new();
            }

        }
        //[Button("Load Language")]
        public void LoadLanguageWithCallback()
        {
            if (SaveManager.ExistsFile(SaveManager.CreatePath
                (Enum.GetName(typeof(SystemLanguage), currentLanguage), TranslateManager.LANGUAGES_SUBFOLDER,
                sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication)))
            {
                Translation = SaveManager.LoadFromFile<Translation>
                (Enum.GetName(typeof(SystemLanguage), currentLanguage),
                subFolder: TranslateManager.LANGUAGES_SUBFOLDER, sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication);
            }
            else
            {
                Debug.Log($"Translation {currentLanguage} not found. This is fine");
                Translation = new();
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
#endif
        #endregion
    }
}
