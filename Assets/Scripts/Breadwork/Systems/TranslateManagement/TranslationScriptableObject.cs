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
        [JsonProperty, BoxGroup("Menu")] public string menu_play;
        [JsonProperty, BoxGroup("Menu")] public string menu_options;
        [JsonProperty, BoxGroup("Menu")] public string menu_quit;
    }
    [BurstCompile]
    [CreateAssetMenu(fileName = "Translation_Language", menuName = "Scripts/Translation", order = 1)]
    public class TranslationScriptableObject : ScriptableObject
    {
        [JsonIgnore, SerializeField] private SystemLanguage currentLanguage = SystemLanguage.English;

        public Translation Translation;

        #region Editor
#if UNITY_EDITOR
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
