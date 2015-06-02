using UnityEngine;
using System.Collections;

public class PauseMenuActions : MonoBehaviour {

    CanvasGroup m_canvasGroup;

    private void Start()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            OpenPauseMenu();
        }
    }

    // PAUSE MENY ACTION EVENTS //

    private void OpenPauseMenu()
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
        //
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
