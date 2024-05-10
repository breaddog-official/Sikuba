using NaughtyAttributes;
using Scripts.TranslateManagement;
using Unity.Burst;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts.Core
{
    [CreateAssetMenu(fileName = "GameDataConfig", menuName = "Scripts/GameDataConfig", order = 0), BurstCompile]
    public class GameDataConfig : ScriptableObject
    {
        [BoxGroup("Game")] public bool IsDebug;
        [BoxGroup("Game")] public ApplicationLanguage DefaultLanguage = ApplicationLanguage.English;
        [BoxGroup("Audio Mixers")] public AudioMixerGroup SoundMixer;
        [BoxGroup("Audio Mixers")] public AudioMixerGroup MusicMixer;
        [BoxGroup("Audio Sources Pooling")] public bool IsPooling;
        [BoxGroup("Audio Sources Pooling")] public int PoolContainers = 6;
    }
}

