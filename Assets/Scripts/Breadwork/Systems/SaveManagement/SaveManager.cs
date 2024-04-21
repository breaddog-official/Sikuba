using Unity.Burst;
using UnityEngine;
using System.IO;
using System;

namespace Scripts.SaveManagement
{
    [BurstCompile]
    public static class SaveManager
    {
        public enum Savers
        {
            Json,
            YAML,
            XML,
            Binary,
            WithoutSerialization,
        }
        /// <summary>
        /// <para>Impact of game updates on files</para>
        /// 
        /// <seealso cref="Persistent"/> = <see cref="Application.persistentDataPath"/> <br />
        /// <seealso cref="UpdateWithApplication"/> = <see cref="Application.streamingAssetsPath"/> <br />
        /// </summary>
        public enum UpdateSensitivity
        {
            Persistent,
            UpdateWithApplication,
        }

        public const string DEFAULT_SAVE_SUBFOLDER = "Configs";

        public static ISaveSystem SaveSystem => savers_json;

        #region SaveToFile
        /// <summary>
        /// Saves your value to a file in a subfolder you specify relative to the Resources folder
        /// </summary>
        /// <typeparam name="T">Type of input</typeparam>
        /// <param name="input">Field to save</param>
        /// <param name="fileName">Name of file to save. The name of the saved file. Can be done with or without extension.</param>
        /// <param name="subFolder">Sub folder relative to the Resources folder. For example "Resources/Languages" or "Languages"</param>
        /// <param name="saveSystem">Saver</param>
        public static void SaveToFile<T>(T input, string fileName, Savers saveSystem = Savers.Json, string subFolder = DEFAULT_SAVE_SUBFOLDER, UpdateSensitivity sensitivity = UpdateSensitivity.Persistent)
        {
            SaveToFile(input, fileName, GetSaveSystem(saveSystem), subFolder, sensitivity);
        }
        /// <summary>
        /// Saves your value to a file in a subfolder you specify relative to the Resources folder
        /// </summary>
        /// <typeparam name="T">Type of input</typeparam>
        /// <param name="input">Field to save</param>
        /// <param name="fileName">Name of file to save. The name of the saved file. Can be done with or without extension.</param>
        /// <param name="subFolder">Sub folder relative to the Resources folder. For example "Resources/Languages" or "Languages"</param>
        /// <param name="saveSystem">Saver</param>
        public static void SaveToFile<T>(T input, string fileName, ISaveSystem saveSystem, string subFolder = DEFAULT_SAVE_SUBFOLDER, UpdateSensitivity sensitivity = UpdateSensitivity.Persistent)
        {
            ISaveSystem localSaveSystem = saveSystem == null ? saveSystem : SaveSystem;
            string path = CreatePath(fileName, subFolder, localSaveSystem, sensitivity);

            localSaveSystem.Save(input, path);
        }
        #endregion
        #region LoadFromFile
        /// <summary>
        /// Loads a file from a subfolder you specify relative to the Resources folder
        /// </summary>
        /// <typeparam name="T">Type of input</typeparam>
        /// <param name="fileName">Name of file to save. The name of the saved file. Can be done with or without extension.</param>
        /// <param name="subFolder">Sub folder relative to the Resources folder. For example "Resources/Languages" or "Languages"</param>
        /// <param name="saveSystem">Saver</param>
        public static T LoadFromFile<T>(string fileName, Savers saveSystem = Savers.Json, string subFolder = DEFAULT_SAVE_SUBFOLDER, UpdateSensitivity sensitivity = UpdateSensitivity.Persistent)
        {
            return LoadFromFile<T>(fileName, GetSaveSystem(saveSystem), subFolder, sensitivity);
        }
        /// <summary>
        /// Loads a file from a subfolder you specify relative to the Resources folder
        /// </summary>
        /// <typeparam name="T">Type of input</typeparam>
        /// <param name="fileName">Name of file to save. The name of the saved file. Can be done with or without extension.</param>
        /// <param name="subFolder">Sub folder relative to the Resources folder. For example "Resources/Languages" or "Languages"</param>
        /// <param name="saveSystem">Saver</param>
        public static T LoadFromFile<T>(string fileName, ISaveSystem saveSystem, string subFolder = DEFAULT_SAVE_SUBFOLDER, UpdateSensitivity sensitivity = UpdateSensitivity.Persistent)
        {
            ISaveSystem localSaveSystem = saveSystem == null ? saveSystem : SaveSystem;
            string path = CreatePath(fileName, subFolder, localSaveSystem, sensitivity);

            if (!ExistsFile(path))
                throw new FileNotFoundException($"File not found ({path})");

            return localSaveSystem.Load<T>(path);
        }
        #endregion

        #region SaveToPath
        /// <summary>
        /// Saves your value to a file in a subfolder you specify relative to the Resources folder
        /// </summary>
        /// <typeparam name="T">Type of input</typeparam>
        /// <param name="input">Field to save</param>
        /// <param name="path">Path to the file to save. With filename.</param>
        /// <param name="saveSystem">Saver</param>
        public static void SaveToPath<T>(T input, string path, Savers saveSystem = Savers.Json)
        {
            SaveToPath(input, path, GetSaveSystem(saveSystem));
        }
        /// <summary>
        /// Saves your value to a file in a subfolder you specify relative to the Resources folder
        /// </summary>
        /// <typeparam name="T">Type of input</typeparam>
        /// <param name="input">Field to save</param>
        /// <param name="path">Path to the file to save. With filename.</param>
        /// <param name="saveSystem">Saver</param>
        public static void SaveToPath<T>(T input, string path, ISaveSystem saveSystem)
        {
            ISaveSystem localSaveSystem = saveSystem == null ? saveSystem : SaveSystem;

            path = CheckExtension(path);
            CheckDirectory(path);

            localSaveSystem.Save(input, path);
        }
        #endregion
        #region LoadFromPath
        /// <summary>
        /// Loads a file from a path you specify
        /// </summary>
        /// <param name="path">Path to the file to load. With filename.</param>
        /// <param name="saveSystem">Saver</param>
        public static T LoadFromPath<T>(string path, Savers saveSystem = Savers.Json)
        {
            return LoadFromPath<T>(path, GetSaveSystem(saveSystem));
        }
        /// <summary>
        /// Loads a file from a path you specify
        /// </summary>
        /// <param name="path">Path to the file to load. With filename.</param>
        /// <param name="saveSystem">Saver</param>
        public static T LoadFromPath<T>(string path, ISaveSystem saveSystem)
        {
            ISaveSystem localSaveSystem = saveSystem == null ? saveSystem : SaveSystem;

            if (!ExistsFile(path))
                throw new FileNotFoundException($"File not found ({path})");

            path = CheckExtension(path);
            CheckDirectory(path);

            return localSaveSystem.Load<T>(path);
        }
        #endregion

        #region GetSaveSystem
        private static readonly JsonSaver savers_json = new();
        private static readonly YamlSaver savers_yaml = new();
        private static readonly XmlSaver savers_xml = new();
        private static readonly BinarySaver savers_binary = new();
        private static readonly WithoutSerializationSaver savers_without_serialization = new();

        public static ISaveSystem GetSaveSystem(Savers saver)
        {
            switch (saver)
            {
                case Savers.Json: return savers_json;
                case Savers.YAML: return savers_yaml;
                case Savers.XML: return savers_xml;
                case Savers.Binary: return savers_binary;
                case Savers.WithoutSerialization: return savers_without_serialization;
                default: return SaveSystem;
            }
        }
        public static ISaveSystem GetSaveSystem<T>() where T : ISaveSystem
        {
            Type type = typeof(T);

            if (type == typeof(JsonSaver)) return savers_json;
            else if (type == typeof(YamlSaver)) return savers_yaml;
            else if (type == typeof(XmlSaver)) return savers_xml;
            else if (type == typeof(BinarySaver)) return savers_binary;
            else if (type == typeof(WithoutSerializationSaver)) return savers_without_serialization;
            else return savers_json;
        }
        #endregion


        #region Checks
        /// <summary>
        /// Checks if a file exists at this path
        /// </summary>
        public static bool ExistsFile(string path)
        {
            return File.Exists(path);
        }
        public static string CheckExtension(string path, ISaveSystem saveSystem = null)
        {
            if (saveSystem == null) saveSystem = SaveSystem;

            if (!Path.HasExtension(path) && Path.GetFileName(path) != "")
                return path += saveSystem.FileExtension;
            else
                return path;
        }
        public static void CheckDirectory(string path)
        {
            string directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        #endregion
        /// <summary>
        /// Creates a platform-specific file path to the save folder and adds an extension (if missing)
        /// </summary>
        public static string CreatePath(string fileName, string subFolder = DEFAULT_SAVE_SUBFOLDER, ISaveSystem saveSystem = null, UpdateSensitivity sensitivity = UpdateSensitivity.Persistent)
        {
            if (saveSystem == null) saveSystem = SaveSystem;

            string newPath = CheckExtension
                (Path.Combine(GetSaveFolderPath(sensitivity), subFolder, Path.GetFileName(fileName)), saveSystem);
            CheckDirectory(newPath); 

            return newPath;
        }
        /// <summary>
        /// Creates a platform-specific file path to the save folder
        /// </summary>
        public static string CreatePath(string subFolder = DEFAULT_SAVE_SUBFOLDER, UpdateSensitivity sensitivity = UpdateSensitivity.Persistent)
        {
            string newPath = Path.Combine(GetSaveFolderPath(sensitivity), subFolder);
            CheckDirectory(newPath);

            return newPath;
        }
        /// <summary>
        /// Returns a path to save folder depending on the platform
        /// </summary>
        public static string GetSaveFolderPath(UpdateSensitivity sensitivity)
        {
            if (sensitivity == UpdateSensitivity.UpdateWithApplication) return Application.streamingAssetsPath;
#if UNITY_EDITOR
            else return Path.Combine(Application.dataPath, "EditorSaves", "Editor");
#else
            else return Application.persistentDataPath;
#endif

        }
    }
}
