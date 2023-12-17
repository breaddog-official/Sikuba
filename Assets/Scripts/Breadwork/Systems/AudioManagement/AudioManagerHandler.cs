using Unity.Burst;
using UnityEngine;

[BurstCompile]
public class AudioManagerHandler : MonoBehaviour
{
    public AudioSource[] Sources { get; set; }
}
