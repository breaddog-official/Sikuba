using Mirror;
using NTC.Pool;
using Unity.Burst;
using UnityEngine;

namespace Scripts.Gameplay
{
    [BurstCompile]
    public class Bullet : NetworkBehaviour, ISpawnable
    {
        [SerializeField] protected float speed = 10.0f;
        [SerializeField] protected float lifetime = 5.0f;
        [SerializeField] protected float damage = 10.0f;
        [SerializeField] protected int hits = 1;
        [Space(10.0f)]
        [SerializeField] protected LayerMask layerMask;

        protected int curHits;
        protected Rigidbody _rb;

        public void OnSpawn()
        {
            curHits = hits;
            _rb = GetComponent<Rigidbody>();
            Invoke(nameof(Destroy), lifetime);
        }
        private void Destroy()
        {
            NightPool.Despawn(this);
            NetworkServer.UnSpawn(gameObject);
        }
        private void FixedUpdate()
        {
            _rb.MovePosition(Vector3.Lerp(transform.position, transform.position + transform.forward, Time.fixedDeltaTime * speed));
        }
        private void OnTriggerEnter(Collider other)
        {
            if ((layerMask & (1 << other.gameObject.layer)) != 0)
            {
                if (TryGetComponent(out IDamageable damageable))
                {
                    Debug.Log("hurt");
                    damageable.Hurt(damage);
                }
                curHits--;
                if (curHits == 0)
                {
                    Destroy();
                }
            }
        }
    }
}
