using System;
using Unity.Burst;
using UnityEditor;
using UnityEngine;

namespace Scripts.TranslateManagement
{
    [BurstCompile]
    public class TranslationCreaterSaveChangesWindow : EditorWindow
    {
        public static bool HasInstance;
        private SystemLanguage languageToSave;
        private GUILayoutOption width = GUILayout.Width(100.0f);

        public event Action OnApply;
        public event Action OnDiscard;

        public static void CustomReset()
        {
            HasInstance = false;
        }
        public static TranslationCreaterSaveChangesWindow OpenWindow(SystemLanguage language)
        {
            TranslationCreaterSaveChangesWindow spawnedWindow = GetWindowWithRect<TranslationCreaterSaveChangesWindow>(new Rect(100.0f, 100.0f, 400.0f, 100.0f), true, "Save changes?", true);

            HasInstance = true;

            spawnedWindow.Show();
            spawnedWindow.languageToSave = language;

            spawnedWindow.OnApply += spawnedWindow.Close;
            spawnedWindow.OnDiscard += spawnedWindow.Close;

            return spawnedWindow;
        }
        private void OnGUI()
        {
            GUILayout.Label($"Save changes in {Enum.GetName(typeof(SystemLanguage), languageToSave)}?");

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save", width)) OnApply?.Invoke();
            if (GUILayout.Button("Discard", width)) OnDiscard?.Invoke();
            GUILayout.EndHorizontal();
        }
        private void OnDestroy()
        {
            CustomReset();
        }
    }
}
