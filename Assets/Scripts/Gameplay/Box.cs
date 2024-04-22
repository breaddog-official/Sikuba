using Mirror;
using NaughtyAttributes;
using Scripts.Gameplay;
using System;
using UnityEngine;

public class Box : NetworkBehaviour, IDamageable
{
    [field: SerializeField, SyncVar, ProgressBar("Health", MAX_HEALTH, EColor.Red)]
    public float Health { get; private set; } = MAX_HEALTH;

    private const float MAX_HEALTH = 50.0f;

    void Start()
    {
        if (isServer) NetworkServer.Spawn(gameObject);
    }
    private void OnDestroy()
    {
        if (isServer) NetworkServer.Destroy(gameObject);
    }

    [Command]
    public void Heal(float value)
    {
        Health = Mathf.Clamp(Health + Math.Abs(value), 0.0f, MAX_HEALTH);
    }
    [Command]
    public void Hurt(float damage)
    {
        Health = Mathf.Clamp(Health - Math.Abs(damage), 0.0f, MAX_HEALTH);

        if (Health == 0.0f) Dead();
    }
    [ClientRpc]
    private void Dead()
    {
        Destroy(this);
    }
}
