using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuActions : MonoBehaviour {

    [SerializeField] public GameObject serverList;

    private CanvasGroup m_canvasGroup;
    private bool m_canResume;

    private void Start()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        if(!Network.isClient || !Network.isServer)
        {
            transform.FindChild("Buttons").FindChild("Button Start").GetComponent<Button>().interactable = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
        if (!m_canResume)
        {
            if (!Network.isClient || !Network.isServer)
            {
                transform.FindChild("Buttons").FindChild("Button Start").GetComponent<Button>().interactable = false;
                m_canResume = true;
            }
        }
    }

    // PAUSE MENY ACTION EVENTS //

    public void OpenPauseMenu()
    {
        m_canvasGroup.alpha = 1;
        m_canvasGroup.interactable = true;
        m_canvasGroup.blocksRaycasts = true;
    }

	public void ResumeGame()
    {
        m_canvasGroup.alpha = 0;
        m_canvasGroup.interactable = false;
        m_canvasGroup.blocksRaycasts = false;
    }

    public void ShowServerList()
    {
        ResumeGame();
        if(serverList != null)
        {
            serverList.GetComponent<ServerList>().OpenServerList();
        }
    }

    public void ShowOptions()
    {
        //
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
