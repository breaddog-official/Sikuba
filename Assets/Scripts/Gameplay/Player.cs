using Mirror;
using NaughtyAttributes;
using Scripts.InputManagement;
using UnityEngine;

public class Player : NetworkBehaviour, IDamageable
{
    private const float MAX_HEALTH = 100.0f;
    [field: SerializeField, BoxGroup("Health"), ProgressBar("Health", MAX_HEALTH, EColor.Red)] 
    public float Health { get; private set; } = MAX_HEALTH;

    protected void Start()
    {
        
    }
    protected void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            
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
