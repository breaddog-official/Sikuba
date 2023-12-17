using UnityEditor;
using UnityEngine;
using System.IO;
using Scripts.SaveManagement;
using System.Collections.Generic;
using System;
using Unity.Burst;

namespace Scripts.TranslateManagement
{
    [BurstCompile]
    public class TranslationCreater : EditorWindow
    {
        private SystemLanguage currentLanguage = SystemLanguage.English;
        private List<string> currentTranslation;

        private bool showAvailableLanguages = true;
        private List<string> filesInLanguageFolder = new List<string>();

        private GUILayoutOption buttonWidth = GUILayout.Width(100.0f);
        private GUILayoutOption smallButtonWidth = GUILayout.Width(20.0f);

        [MenuItem("Services/Transltation Creater")]
        public static void OpenWindow()
        {
            GetWindow<TranslationCreater>("Translation Creater").Show();
        }
        private void OnBecameVisible()
        {
            UpdateLanguage();
            dropdownLanguage = currentLanguage;
        }

        SystemLanguage dropdownLanguage;
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            dropdownLanguage = (SystemLanguage)EditorGUILayout.EnumPopup("Currently edited language:", dropdownLanguage);
            if (currentLanguage != dropdownLanguage && !TranslationCreaterSaveChangesWindow.HasInstance)
            {
                TranslationCreaterSaveChangesWindow window = TranslationCreaterSaveChangesWindow.OpenWindow(currentLanguage);

                window.OnApply += SaveChanges;
                window.OnDiscard += DiscardChanges;
            }

            bool tryLoadTranslationResult = TryLoadTranslation();
            if (GUILayout.Button(tryLoadTranslationResult ? "Load" : "Save", buttonWidth))
            {
                UpdateLanguage(tryLoadTranslationResult);
            }
            if (tryLoadTranslationResult)
            {
                if (GUILayout.Button("Save", buttonWidth))
                {
                    SaveTranslation();
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5.0f);
            EditorGUILayout.BeginHorizontal();

            showAvailableLanguages = EditorGUILayout.Foldout(showAvailableLanguages, "AvailableLanguages" + (showAvailableLanguages ? ":" : ""));
            if (showAvailableLanguages)
            {
                if (GUILayout.Button("Update", buttonWidth))
                {
                    UpdateFilesInLanguageFolder();
                }

                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10.0f);

                foreach (string s in filesInLanguageFolder)
                {
                    GUILayout.Label(s);
                }
                GUILayout.Space(10.0f);
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"Size: {currentTranslation.Count}");

            if (GUILayout.Button("+", smallButtonWidth)) ChangeTranslationSize(1);
            else if (GUILayout.Button("-", smallButtonWidth)) ChangeTranslationSize(-1);

            EditorGUILayout.EndHorizontal();

            RenderList(currentTranslation);
        }
        public override void DiscardChanges()
        {
            base.DiscardChanges();
            currentLanguage = dropdownLanguage;
            UpdateLanguage();
        }
        public override void SaveChanges()
        {
            base.SaveChanges();
            SaveTranslation(currentLanguage);
            currentLanguage = dropdownLanguage;
            UpdateLanguage();
        }
        private void UpdateLanguage()
        {
            if (TryLoadTranslation())
                currentTranslation = LoadTranslation();
            else
                SaveTranslation();

            UpdateFilesInLanguageFolder();
        }
        private void UpdateLanguage(bool tryLoadTranslation)
        {
            if (tryLoadTranslation)
                currentTranslation = LoadTranslation();
            else
                SaveTranslation();

            UpdateFilesInLanguageFolder();
        }
        private void ChangeTranslationSize(int value)
        {
            if (value == 0) return;
            else if (value > 0) currentTranslation.AddRange(new string[value]);
            else if (value < 0) currentTranslation.RemoveRange(currentTranslation.Count - 1, Math.Abs(value));
        }
        private void UpdateFilesInLanguageFolder()
        {
            string[] files = Directory.GetFiles(SaveManager.CreatePath(string.Empty, TranslateManager.LANGUAGES_SUBFOLDER));
            filesInLanguageFolder.Clear();
            foreach (string s in files)
            {
                if (Path.GetExtension(s) == ".meta") continue;

                filesInLanguageFolder.Add(Path.GetFileNameWithoutExtension(s));
            }
        }
        private void RenderList(List<string> list)
        {
            GUILayout.BeginScrollView(new Vector2(0.0f, 0.0f));

            for (int i = 0; i < list.Count; i++)
            {
                GUILayout.Label($"{i}:");
                currentTranslation[i] = EditorGUILayout.TextField(currentTranslation[i]);
            }

            GUILayout.EndScrollView();
        }
        private bool TryLoadTranslation()
        {
            return SaveManager.ExistsFile(SaveManager.CreatePath(Enum.GetName(typeof(SystemLanguage), currentLanguage), TranslateManager.LANGUAGES_SUBFOLDER, SaveManager.GetSaveSystem(SaveManager.Savers.YAML)));
        }
        private List<string> LoadTranslation(SystemLanguage language = SystemLanguage.Unknown)
        {
            if (language == SystemLanguage.Unknown)
                language = currentLanguage;

            List<string> loadedTranslation = SaveManager.LoadFromFile<List<string>>(Enum.GetName(typeof(SystemLanguage), language), SaveManager.Savers.YAML, TranslateManager.LANGUAGES_SUBFOLDER);
            Debug.Log($"{language} was loaded.");
            return loadedTranslation;
        }
        private void SaveTranslation(SystemLanguage language = SystemLanguage.Unknown)
        {
            if (language == SystemLanguage.Unknown)
                language = currentLanguage;

            SaveManager.SaveToFile(currentTranslation, Enum.GetName(typeof(SystemLanguage), language), SaveManager.Savers.YAML, TranslateManager.LANGUAGES_SUBFOLDER);
            Debug.Log($"{language} was saved.");
        }
    }
}
