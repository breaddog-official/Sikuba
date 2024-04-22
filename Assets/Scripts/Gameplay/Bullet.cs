using Mirror;
using Unity.Burst;
using UnityEngine;

namespace Scripts.Gameplay
{
    [BurstCompile]
    public class Bullet : NetworkBehaviour
    {
        [SerializeField] protected float speed = 10.0f;
        [SerializeField] protected float lifetime = 5.0f;
        [SerializeField] protected float damage = 10.0f;
        [SerializeField] protected int hits = 1;
        [Space(10.0f)]
        [SerializeField] protected LayerMask layerMask;

        protected int curHits;
        protected Rigidbody _rb;

        private void Start()
        {
            curHits = hits;
            _rb = GetComponent<Rigidbody>();
        }
        public override void OnStartServer()
        {
            Invoke(nameof(Destroy), lifetime);
        }
        [Server]
        private void Destroy()
        {
            NetworkServer.Destroy(gameObject);
        }
        private void FixedUpdate()
        {
            _rb.MovePosition(Vector3.Lerp(transform.position, transform.position + transform.forward, Time.fixedDeltaTime * speed));
        }
        [ServerCallback]
        private void OnTriggerEnter(Collider other)
        {
            if ((layerMask & (1 << other.gameObject.layer)) != 0)
            {
                if (other.TryGetComponent(out IDamageable damageable))
                {
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
