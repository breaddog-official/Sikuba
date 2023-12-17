using Mirror;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float lifetime;
    [SerializeField] protected float damage;
    [SerializeField] protected int hits = 1;
    [Space(10.0f)]
    [SerializeField] protected LayerMask layerMask;

    private void Start()
    {
        Invoke(nameof(Destroy), lifetime);
    }
    private void Destroy() => NetworkServer.Destroy(gameObject);
    private void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, transform.position + transform.right, Time.fixedDeltaTime * speed);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == layerMask)
        {
            if (TryGetComponent(out IDamageable damageable))
            {
                damageable.ChangeHealth(-damage);
                hits--;
                if (hits == 0) Destroy();
            }
        }
    }
}
