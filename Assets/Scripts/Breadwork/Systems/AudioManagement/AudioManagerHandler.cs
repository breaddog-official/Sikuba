using Unity.Burst;
using UnityEngine;

namespace Scripts.Audio
{
    [BurstCompile]
    public class AudioManagerHandler : MonoBehaviour
    {
        public AudioSource[] Sources { get; set; }
    }
}
