using Mirror;

namespace Scripts.Gameplay
{
    public interface IDamageable
    {
        public float Health { get; }

        [Command, Server]
        void Heal(float value);
        [Command, Server]
        void Hurt(float damage);
    }
}
