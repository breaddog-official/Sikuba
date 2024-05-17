using UnityEngine;

public class ErrorMessageFabric : MonoBehaviour
{
    [field: SerializeField] public ErrorMessage Prefab { get; private set; }

    public ErrorMessageFabric() => Instance = this;
    protected static ErrorMessageFabric Instance;
    public static ErrorMessage CreateInstance(string title, string message)
    {
        ErrorMessage errorMessage = Instantiate(Instance.Prefab, parent: Instance.transform);
        errorMessage.Initialize(title, message);
        return errorMessage;
    }
}
