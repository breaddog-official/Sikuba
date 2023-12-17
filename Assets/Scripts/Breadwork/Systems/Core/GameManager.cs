using Unity.Burst;
using UnityEngine;
using System;
using Scripts.Settings;
using System.Collections;
using Scripts.TranslateManagement;
using Scripts.InputManagement;

namespace Scripts.Core
{
    [BurstCompile]
    public class GameManager : MonoBehaviour
    {
        public static GameDataConfig GameDataConfig { get; private set; }
        public static GameManager Instance { get; private set; }

        private void Awake() => Initialize();
        public void Initialize()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            // DontDestroyOnLoad(this);     Mirror's Network Manager do this

            GameDataConfig = Resources.Load<GameDataConfig>("GameDataConfig");

            TranslateManager.Initialize();
            InputManager.LoadControlsOverrides();
        }
        private void Start()
        {
            TranslateManager.ChangeLanguage(SettingsManager.ReadValue<SystemLanguage>(5));
        }
        public static int GetElementIndexInArray<T>(T element, IEnumerator array)
        {
            array.Reset();
            int i = 0;
            do
            {
                if (Equals(element, array.Current)) return i;
                i++;
            } 
            while (!array.MoveNext());
            return -1;
        }
        public static int GetElementIndexInArray<T>(T element, T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (Equals(element, array[i])) return i;
            }
            return -1;
        }
        public static T AdvancedConvert<T>(object value)
        {
            try
            {
                return (T)value;
            }
            catch
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }
    }
}
