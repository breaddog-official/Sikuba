using Mirror;
using Mirror.FizzySteam;
using TMPro;
using Unity.Burst;
using UnityEngine;

namespace Scripts.UI
{
    [BurstCompile]
    public class SteamIDInputField : MonoBehaviour
    {
        private enum SteamIdFieldMode
        {
            Create,
            Join,
        }
        [SerializeField] private SteamIdFieldMode mode;
        private TMP_InputField inputField;

        private void Start()
        {
            inputField = GetComponent<TMP_InputField>();

            if (mode == SteamIdFieldMode.Create)
                inputField.text = NetworkManager.singleton.GetComponent<FizzyFacepunch>().SteamUserID.ToString();
        }

        public void CopyIdToBuffer()
        {
            GUIUtility.systemCopyBuffer = inputField.text;
        }
        public void PasteIdToInputField()
        {
            inputField.text = GUIUtility.systemCopyBuffer;
        }
    }
}
