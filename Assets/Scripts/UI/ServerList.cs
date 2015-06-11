using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerList : MonoBehaviour {

    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private GameObject serverListWrapper;
    [SerializeField] private CanvasGroup m_canvasGroup;

    private List<GameObject> m_panels;
    private bool listNeedsRebuild;
    private ServerPanel m_waitingServerPanel;

    private void Start()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        DestroyList();
        StartCoroutine(NetworkManager.Instance.RefreshHostList());
        listNeedsRebuild = true;
    }

    public void OnConnectedToServer()
    {
        if(m_waitingServerPanel != null)
        {
            m_waitingServerPanel.StartConnection();
        }
    }

    private void Update()
    {
        if(listNeedsRebuild)
        {
            if (!NetworkManager.Instance.isRefreshing)
            {
                for (int i = 0; i < NetworkManager.Instance.hostData.Length; i++)
                {
                    GameObject panel = GameObject.Instantiate(panelPrefab, Vector3.zero, panelPrefab.transform.rotation) as GameObject;
                    panel.transform.parent = serverListWrapper.transform;
                    panel.GetComponent<ServerPanel>().SetPosition(i);
                    panel.GetComponent<ServerPanel>().SetServer(NetworkManager.Instance.hostData[i]);
                    m_panels.Add(panel);
                }
                listNeedsRebuild = false;
            }
        }
    }

    private void DestroyList()
    {
        if(m_panels != null)
        {
            foreach (GameObject panel in m_panels)
            {
                Destroy(panel.gameObject);
            }
        }
        m_panels = new List<GameObject>();
    }

    public void SetPanelToWaiting(ServerPanel panel)
    {
        m_waitingServerPanel = panel;
    }

    [RPC]
    public void ReceiveResponce(int responce)
    {
        m_waitingServerPanel.ReceiveResponce(responce);
    }

    public void BackToMain()
    {
        CanvasManager.Instance.pauseMenu.GetComponent<PauseMenuActions>().OpenPauseMenu();
        m_canvasGroup.alpha = 0;
        m_canvasGroup.interactable = false;
        m_canvasGroup.blocksRaycasts = false;
    }

    public void OpenServerList()
    {
        m_canvasGroup.alpha = 1;
        m_canvasGroup.interactable = true;
        m_canvasGroup.blocksRaycasts = true;
    }
}
