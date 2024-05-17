using UnityEditor;
using UnityEngine;
using System.IO;
using Scripts.SaveManagement;
using System.Collections.Generic;
using System;
using Unity.Burst;
using System.Reflection;
using static UnityEditor.PlayerSettings.Switch;

namespace Scripts.TranslateManagement
{
    [BurstCompile]
    public class TranslationCreater : EditorWindow
    {
        private ApplicationLanguage currentLanguage = ApplicationLanguage.English;
        private Translation currentTranslation;

        private bool showAvailableLanguages = true;
        private List<string> filesInLanguageFolder = new List<string>();

        private readonly GUILayoutOption buttonWidth = GUILayout.Width(100.0f);
        private readonly bool PRINT_DEBUG_INFO = false;

        private FieldInfo[] translationFields = typeof(Translation).GetFields();
        #region GUI
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

        Vector2 renderLanguagesScrollView = new Vector2(0.0f, 0.0f);
        ApplicationLanguage dropdownLanguage;
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            dropdownLanguage = (ApplicationLanguage)EditorGUILayout.EnumPopup("Currently edited language:", dropdownLanguage);
            if (currentLanguage != dropdownLanguage && !TranslationCreaterSaveChangesWindow.HasInstance)
            {
                if (ExistsTranslation(currentLanguage) && ExistsTranslation(dropdownLanguage))
                {
                    if (!CompareTranslations(currentTranslation, LoadTranslation(currentLanguage)))
                    {
                        TranslationCreaterSaveChangesWindow window = TranslationCreaterSaveChangesWindow.OpenWindow(currentLanguage);
                        window.OnApply += SaveChanges;
                        window.OnDiscard += DiscardChanges;
                    }
                    else DiscardChanges();
                }
                else DiscardChanges();
            }

            bool tryLoadTranslationResult = ExistsTranslation(currentLanguage);
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

                renderLanguagesScrollView = GUILayout.BeginScrollView(renderLanguagesScrollView);
                foreach (string s in filesInLanguageFolder)
                {
                    GUILayout.Label(s);
                }
                GUILayout.EndScrollView();
                GUILayout.Space(10.0f);
            }
            else
            {
                EditorGUILayout.EndHorizontal();
            }

            RenderTranslation(currentTranslation);
        }
        private void RenderList(List<string> list)
        {
            renderTranlsationScrollView = GUILayout.BeginScrollView(renderTranlsationScrollView);

            for (int i = 0; i < list.Count; i++)
            {
                GUILayout.Label($"{i}:");
                list[i] = EditorGUILayout.TextField(list[i]);
            }

            GUILayout.EndScrollView();
        }
        Vector2 renderTranlsationScrollView = new Vector2(0.0f, 0.0f);
        private void RenderTranslation(Translation translation)
        {
            renderTranlsationScrollView = GUILayout.BeginScrollView(renderTranlsationScrollView);

            for (int i = 0; i < translationFields.Length; i++)
            {
                GUILayout.Label($"{translationFields[i].Name}:");
                translationFields[i].SetValue(translation, EditorGUILayout.TextField((string)translationFields[i].GetValue(translation)));
            }

            GUILayout.EndScrollView();
        }
        #endregion
        #region SaveManagement
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
            if (ExistsTranslation(currentLanguage))
                currentTranslation = LoadTranslation();
            else
                currentTranslation = new();

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
        private bool ExistsTranslation(ApplicationLanguage language)
        {
            return SaveManager.ExistsFile(SaveManager.CreatePath(Enum.GetName(typeof(ApplicationLanguage), language), TranslateManager.LANGUAGES_SUBFOLDER, sensitivity: SaveManager.UpdateSensitivity.UpdateWithApplication));
        }
        private bool CompareTranslations(Translation first, Translation second)
        {
            translationFields = typeof(Translation).GetFields();
            for (int i = 0; i < translationFields.Length; i++)
            {
                if (!Equals(translationFields[i].GetValue(first), translationFields[i].GetValue(second)))
                {
                    return false;
                }
            }
            return true;
        }
        private Translation LoadTranslation()
        {
            Translation loadedTranslation = TranslateManager.LoadTranslation(currentLanguage);
            if (PRINT_DEBUG_INFO) Debug.Log($"{currentLanguage} was loaded.");
            return loadedTranslation;
        }
        private Translation LoadTranslation(ApplicationLanguage language)
        {
            Translation loadedTranslation = TranslateManager.LoadTranslation(language);
            if (PRINT_DEBUG_INFO) Debug.Log($"{language} was loaded.");
            return loadedTranslation;
        }
        private void SaveTranslation()
        {
            TranslateManager.SaveTranslation(currentTranslation, currentLanguage);
            if (PRINT_DEBUG_INFO) Debug.Log($"{currentLanguage} was saved.");
        }
        private void SaveTranslation(ApplicationLanguage language)
        {
            TranslateManager.SaveTranslation(currentTranslation, language);
            if (PRINT_DEBUG_INFO) Debug.Log($"{language} was saved.");
        }
        #endregion
    }
}
