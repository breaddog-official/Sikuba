using System;
using Unity.Burst;
using UnityEngine;

namespace Scripts.TranslateManagement
{
    [BurstCompile, Serializable]
    public class Translation
    {
        [Header("Global")]
        public string back;
        [Header("Menu")]
        public string menu_play;
        public string menu_options;
        public string menu_quit;
        [Header("Menu: Connection")]
        public string menu_connection_create;
        public string menu_connection_join;
        [Header("Menu: Room Options")]
        public string menu_roomOptions_networkAddress;
        public string menu_roomOptions_steamId;
        public string menu_roomOptions_port;
        public string menu_roomOptions_maxConnections;
        public string menu_roomOptions_connectionType;
        [Header("Menu: Options")]
        public string menu_options_language;
        public string menu_options_resolution;
        public string menu_options_fullscreenMode;
        public string menu_options_refreshRate;
        public string menu_options_pcConfig;
        public string menu_options_gamepadConfig;
    }
}
