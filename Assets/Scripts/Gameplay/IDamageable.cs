namespace Scripts.Gameplay
{
    public interface IDamageable
    {
        public float Health { get; }

        void Heal(float value);
        void Hurt(float damage);
    }
}
