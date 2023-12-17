using NaughtyAttributes;
using UnityEngine;

namespace Scripts.Audio
{
    [System.Serializable]
    public struct Sound
    {
        [Required]
        public AudioClip audioClip;
        [Range(0.0f, 1.0f)]
        public float volume;
        public bool loop;
        public AudioManager.MixerType type;
    }
}
