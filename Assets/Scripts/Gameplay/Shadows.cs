using NaughtyAttributes;
using UnityEngine;

public class Shadows : MonoBehaviour
{
    [SerializeField, BoxGroup("Shadows")] protected Shadow[] shadows;

    protected void LateUpdate()
    {
        UpdateShadow();
    }
    protected void UpdateShadow()
    {
        foreach (var shadow in shadows) shadow.UpdateShadow();
    }
}
[System.Serializable]
public struct Shadow
{
    [SerializeField, BoxGroup("Shadow")] public Transform shadow;
    [SerializeField, BoxGroup("Shadow")] public Transform linkedObject;
    [SerializeField, BoxGroup("Shadow")] public Vector3 shadowOffset;

    public readonly void UpdateShadow() => shadow.position = linkedObject.position + shadowOffset;
}
