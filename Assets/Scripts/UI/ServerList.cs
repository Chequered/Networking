using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerList : MonoBehaviour {

    [SerializeField] private GameObject panelPrefab;
    [SerializeField] private GameObject serverListWrapper;
    [SerializeField] private CanvasGroup m_canvasGroup;

    private List<ServerPanel> m_panels;
    private bool listNeedsRebuild;

    private void Start()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        m_panels = new List<ServerPanel>();
        StartCoroutine(NetworkManager.Instance.RefreshHostList());
        DestroyList();
        listNeedsRebuild = true;
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
                }
                listNeedsRebuild = false;
            }
        }
    }

    private void DestroyList()
    {
        foreach (ServerPanel panel in m_panels)
        {
            Destroy(panel.gameObject);
        }
    }

    public void BackToMain()
    {
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
