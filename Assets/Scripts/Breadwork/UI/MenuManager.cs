using UnityEngine;
using NaughtyAttributes;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using kcp2k;
using Mirror;
using Mirror.FizzySteam;
using TMPro;
using Unity.Burst;

namespace Scripts.UI
{
    [BurstCompile]
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private CanvasGroup currentTab;
        [SerializeField] private CanvasGroupStruct[] groups;
        [SerializeField] private float lerpSpeed = 0.08f;

        [SerializeField, BoxGroup("ConnectionMenu")] private GameObject[] AddressInputFields;
        [SerializeField, BoxGroup("ConnectionMenu")] private GameObject[] SteamIdes;
        [SerializeField, BoxGroup("ConnectionMenu")] private GameObject[] SteamIdes_SubTitles;
        [SerializeField, BoxGroup("ConnectionMenu")] private GameObject[] PortInputFields;

        private bool isBusy;

        public void Quit() => Application.Quit();
        public void ChangeScene(int scene) => SceneManager.LoadScene(scene);
        private void Start()
        {
            ApplyCanvasGroup(GetCanvasGroupStruct(currentTab));

            steamTransport = NetworkManager.singleton.GetComponent<FizzyFacepunch>();
            localTransport = NetworkManager.singleton.GetComponent<ThreadedKcpTransport>();
        }

        #region Networking
        private FizzyFacepunch steamTransport;
        private ThreadedKcpTransport localTransport;

        public void SetAddress(string address) => NetworkManager.singleton.networkAddress = address;
        public void SetPort(string port) => localTransport.Port = (ushort)int.Parse(port);
        public void SetMaxConnections(float connections) => NetworkManager.singleton.maxConnections = (int)connections;
        public void SetConnectionType(int connectionType)
        {
            switch (connectionType)
            {
                case 0:
                    steamTransport.enabled = false;
                    localTransport.enabled = true;

                    SetActiveLinks(true, true, false);
                    return;
                case 1:
                    steamTransport.enabled = true;
                    localTransport.enabled = false;

                    SetActiveLinks(false, false, true);
                    return;
                case 2:
                    steamTransport.enabled = true;
                    localTransport.enabled = true;

                    SetActiveLinks(false, true, true, true);
                    return;
            }
        }
        private void SetActiveLinks(bool addresses, bool ports, bool steamides, bool steamidesSubTitles = false)
        {
            foreach (GameObject inputField in AddressInputFields) inputField.SetActive(addresses);
            foreach (GameObject inputField in PortInputFields) inputField.SetActive(ports);
            foreach (GameObject inputField in SteamIdes) inputField.SetActive(steamides);
            foreach (GameObject subTitle in SteamIdes_SubTitles) subTitle.SetActive(steamidesSubTitles); 
        }

        public void CreateRoom() => NetworkManager.singleton.StartHost();
        public void JoinRoom() => NetworkManager.singleton.StartClient();
        #endregion

        public void ApplyCanvasGroup(CanvasGroupStruct group)
        {
            EventSystem.current.SetSelectedGameObject(group.SelectedGameObject);
        }
        public void ChangeTab(CanvasGroup newTab)
        {
            if (!isBusy)
            {
                StartCoroutine(LerpFromTabToTab(newTab));
            }
                
        }

        public IEnumerator LerpFromTabToTab(CanvasGroup newTab)
        {
            isBusy = true;
            newTab.gameObject.SetActive(true);
            if (currentTab != newTab) newTab.alpha = 0.0f;

            while (currentTab.alpha > 0.01f)
            {
                currentTab.alpha -= lerpSpeed * Time.deltaTime;
                yield return null;
            }
            GetCanvasGroupStruct(currentTab).SelectedGameObject = EventSystem.current.currentSelectedGameObject;
            ApplyCanvasGroup(GetCanvasGroupStruct(newTab));
            while (newTab.alpha < 1.0f)
            {
                newTab.alpha += lerpSpeed * Time.deltaTime;
                yield return null;
            }
            if (currentTab != newTab)
                currentTab.gameObject.SetActive(false);

            currentTab = newTab;
            isBusy = false;
        }
        public CanvasGroupStruct GetCanvasGroupStruct(CanvasGroup group)
        {
            for (int i = 0; i < groups.Length; i++)
            {
                if (group.GetInstanceID() == groups[i].ÑanvasGroup.GetInstanceID())
                {
                    return groups[i];
                }
            }
            return default;
        }
    }
    [Serializable]
    public class CanvasGroupStruct
    {
        [field: SerializeField] public CanvasGroup ÑanvasGroup { get; set; }
        [field: SerializeField] public GameObject SelectedGameObject { get; set; }
    }
}
