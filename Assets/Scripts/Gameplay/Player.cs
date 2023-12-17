using Mirror;
using NaughtyAttributes;
using UnityEngine;

public class Player : NetworkBehaviour, IDamageable
{
    private const float MAX_HEALTH = 100.0f;
    [field: SerializeField, BoxGroup("Health"), ProgressBar("Health", MAX_HEALTH, EColor.Red)] public float Health { get; private set; } = MAX_HEALTH;

    [field: SerializeField, BoxGroup("Links")] public PlayerMovement player_movement;
    [field: SerializeField, BoxGroup("Links")] public PlayerGun player_gun;

    protected void Start()
    {
        if (player_movement == null) TryGetComponent(out player_movement);
        if (player_gun == null) TryGetComponent(out player_gun);
    }
    protected void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if (player_movement != null) player_movement.Move();
            if (player_gun != null) player_gun.RotateGun();
        }
    }
    public void ChangeHealth(float value)
    {
        Health = Mathf.Clamp(Health + value, 0.0f, MAX_HEALTH);
    }
}
public interface IDamageable
{
    public float Health { get; }
    void ChangeHealth(float value);
}
