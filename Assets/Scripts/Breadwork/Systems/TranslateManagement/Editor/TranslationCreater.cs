using UnityEditor;
using UnityEngine;
using System.IO;
using Scripts.SaveManagement;
using System.Collections.Generic;
using System;
using Unity.Burst;
using System.Reflection;

namespace Scripts.TranslateManagement
{
    [BurstCompile]
    public class TranslationCreater : EditorWindow
    {
        private SystemLanguage currentLanguage = SystemLanguage.English;
        private Translation currentTranslation;

        private bool showAvailableLanguages = true;
        private List<string> filesInLanguageFolder = new List<string>();

        private readonly GUILayoutOption buttonWidth = GUILayout.Width(100.0f);
        private readonly FieldInfo[] translationFields = typeof(Translation).GetFields();

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

            RenderTranslation(currentTranslation);
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
        private void UpdateFilesInLanguageFolder()
        {
            string[] files = Directory.GetFiles(SaveManager.CreatePath(TranslateManager.LANGUAGES_SUBFOLDER, SaveManager.UpdateSensitivity.UpdateWithApplication));
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
                list[i] = EditorGUILayout.TextField(list[i]);
            }

            GUILayout.EndScrollView();
        }
        private void RenderTranslation(Translation translation)
        {
            GUILayout.BeginScrollView(new Vector2(0.0f, 0.0f));

            for (int i = 0; i < translationFields.Length; i++)
            {
                GUILayout.Label($"{translationFields[i].Name}:");
                translationFields[i].SetValue(currentTranslation, EditorGUILayout.TextField((string)translationFields[i].GetValue(currentTranslation)));
            }

            GUILayout.EndScrollView();
        }
        private bool TryLoadTranslation()
        {
            return SaveManager.ExistsFile(SaveManager.CreatePath(Enum.GetName(typeof(SystemLanguage), currentLanguage), TranslateManager.LANGUAGES_SUBFOLDER, SaveManager.GetSaveSystem(SaveManager.Savers.YAML)));
        }
        private Translation LoadTranslation(SystemLanguage language = SystemLanguage.Unknown)
        {
            if (language == SystemLanguage.Unknown)
                language = currentLanguage;

            Translation loadedTranslation = SaveManager.LoadFromFile<Translation>
                (Enum.GetName(typeof(SystemLanguage), currentLanguage), SaveManager.Savers.Json, TranslateManager.LANGUAGES_SUBFOLDER, SaveManager.UpdateSensitivity.UpdateWithApplication);
            Debug.Log($"{language} was loaded.");
            return loadedTranslation;
        }
        private void SaveTranslation(SystemLanguage language = SystemLanguage.Unknown)
        {
            if (language == SystemLanguage.Unknown)
                language = currentLanguage;

            SaveManager.SaveToFile(currentTranslation, Enum.GetName(typeof(SystemLanguage), currentLanguage),
                SaveManager.Savers.YAML, TranslateManager.LANGUAGES_SUBFOLDER, SaveManager.UpdateSensitivity.UpdateWithApplication);
            Debug.Log($"{language} was saved.");
        }
    }
}
