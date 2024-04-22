using Mirror;
using NaughtyAttributes;
using System;
using Unity.Burst;
using UnityEngine;

namespace Scripts.Gameplay
{
    [BurstCompile]
    public class Player : NetworkBehaviour, IDamageable
    {
        [field: SerializeField, SyncVar, BoxGroup("Health"), ProgressBar("Health", MAX_HEALTH, EColor.Red)]
        public float Health { get; private set; } = MAX_HEALTH;

        private const float MAX_HEALTH = 100.0f;

        [Command]
        public void Heal(float value)
        {
            Health = Mathf.Clamp(Health + Math.Abs(value), 0.0f, MAX_HEALTH);
        }
        [Command]
        public void Hurt(float damage)
        {
            Health = Mathf.Clamp(Health - Math.Abs(damage), 0.0f, MAX_HEALTH);
            Debug.Log(Health);
        }
    }
}
