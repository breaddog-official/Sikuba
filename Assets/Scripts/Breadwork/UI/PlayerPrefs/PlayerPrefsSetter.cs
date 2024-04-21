namespace Scripts.UI.PlayerPrefs
{
    using Unity.Burst;
    using UnityEngine;
    [BurstCompile]
    public abstract class PlayerPrefsSetter<T> : MonoBehaviour
    {
        [field: SerializeField] public string Key { get; protected set; }
        [field: SerializeField] public T DefaultValue { get; protected set; }

        public T Value { get; protected set; }

        protected virtual void OnEnable() { Start(); }
        protected abstract void Start();
        protected virtual void OnValidate()
        {
            Value = DefaultValue;
        }
    }
}
