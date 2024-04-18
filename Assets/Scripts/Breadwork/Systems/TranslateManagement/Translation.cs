using NaughtyAttributes;
using Scripts.SaveManagement;
using Scripts.TranslateManagement;
using System;
using UnityEngine;
using Newtonsoft.Json;
using UnityEditor;
using Newtonsoft.Json.Serialization;

[CreateAssetMenu(fileName = "Translation_Language", menuName = "Scripts/Translation", order = 1)]
public class Translation : ScriptableObject
{
    [JsonIgnore, SerializeField] private SystemLanguage currentLanguage = SystemLanguage.English;

    [JsonProperty, BoxGroup("Menu")] public string menu_play;
    [JsonProperty, BoxGroup("Menu")] public string menu_options;
    [JsonProperty, BoxGroup("Menu")] public string menu_quit;

    #region Editor
#if UNITY_EDITOR
    [Button]
    public void CreateTranslationFromThis()
    {
        SaveManager.SaveToFile(this, Enum.GetName(typeof(SystemLanguage), currentLanguage), SaveManager.Savers.YAML, TranslateManager.LANGUAGES_SUBFOLDER);

        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
        DestroyImmediate(this, true);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
    #endregion
}
