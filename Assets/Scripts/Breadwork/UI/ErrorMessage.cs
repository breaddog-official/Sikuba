using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    [SerializeField] protected TMP_Text titleText;
    [SerializeField] protected TMP_Text messageText;
    [Space]
    [SerializeField] protected Selectable button;

    GameObject cachedSelectedGameObject;

    public void Initialize(string title, string message)
    {
        titleText.SetText(title);
        messageText.SetText(message);

        cachedSelectedGameObject = EventSystem.current.currentSelectedGameObject;
        button.Select();
    }
    public void Close()
    {
        EventSystem.current.SetSelectedGameObject(cachedSelectedGameObject);
        gameObject.SetActive(false);
        Destroy(this, 1.5f);
    }
}
