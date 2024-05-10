using NaughtyAttributes;
using System;
using Newtonsoft.Json;
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
}
