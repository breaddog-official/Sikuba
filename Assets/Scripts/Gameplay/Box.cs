using UnityEngine;

public class Box : MonoBehaviour, IDamageable
{
    public const float MAX_HEALTH = 50.0f;
    public float Health { get; private set; } = MAX_HEALTH;
    public void ChangeHealth(float value)
    {
        Health = Mathf.Clamp(Health + value, 0.0f, MAX_HEALTH);
        if (Health == 0) Destroy(this);
    }
}
