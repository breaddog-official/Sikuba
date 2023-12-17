using Scripts.Core;
using Unity.Burst;
using UnityEngine;

namespace Scripts.Audio
{
    [BurstCompile]
    public static class AudioManager
    {
        public enum MixerType { Sound, Music, };

        private static readonly AudioManagerHandler handler;

        static AudioManager()
        {
            handler = Object.Instantiate(Resources.Load<AudioManagerHandler>("AudioManagerHandler"));
            Object.DontDestroyOnLoad(handler);
            if (!GameManager.GameDataConfig.IsPooling)
                return;

            handler.Sources = new AudioSource[GameManager.GameDataConfig.PoolContainers];
            for (int i = 0; i < GameManager.GameDataConfig.PoolContainers; i++)
            {
                handler.Sources[i] = handler.gameObject.AddComponent<AudioSource>();
            }
        }
        /// <summary>
        /// Plays sound in empty AudioSource and return it
        /// </summary>
        public static AudioSource SetSound(Sound sound)
        {
            foreach (AudioSource aud in handler.Sources)
            {
                if (aud.isPlaying) continue;

                SetSound(sound, aud);
                return aud;
            }
            return SetSoundToNewSource(sound);
        }
        /// <summary>
        /// Plays sound in specified AudioSource
        /// </summary>
        public static void SetSound(Sound sound, AudioSource source)
        {
            source.clip = sound.audioClip;
            source.volume = sound.volume;
            source.loop = sound.loop;
            switch (sound.type)
            {
                case MixerType.Sound:
                    source.outputAudioMixerGroup = GameManager.GameDataConfig.SoundMixer;
                    break;
                case MixerType.Music:
                    source.outputAudioMixerGroup = GameManager.GameDataConfig.MusicMixer;
                    break;
            }
            source.Play();
        }
        /// <summary>
        /// Plays sound in new AudioSource and return it
        /// </summary>
        public static AudioSource SetSoundToNewSource(Sound sound, bool destroySourceAfterPlay = true)
        {
            AudioSource source = handler.gameObject.AddComponent<AudioSource>();
            source.clip = sound.audioClip;
            source.volume = sound.volume;
            source.loop = sound.loop;
            switch (sound.type)
            {
                case MixerType.Sound:
                    source.outputAudioMixerGroup = GameManager.GameDataConfig.SoundMixer;
                    break;
                case MixerType.Music:
                    source.outputAudioMixerGroup = GameManager.GameDataConfig.MusicMixer;
                    break;
            }
            source.Play();

            if (!sound.loop && destroySourceAfterPlay)
                Object.Destroy(source, sound.audioClip.length + 0.5f);
            return source;
        }
        /// <summary>
        /// Stops sound in specified AudioSource
        /// </summary>
        public static void StopSound(AudioSource source)
        {
            source.Stop();
        }
    }
}
