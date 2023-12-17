using UnityEngine;

public abstract class PlayerPrefsSetter<T> : MonoBehaviour
{
    [field: SerializeField] public string Key { get; protected set; }
    [field: SerializeField] public T DefaultValue { get; protected set; }

    public T Value { get; protected set; }

    protected virtual void OnValidate()
    {
        Value = DefaultValue;
    }
}
