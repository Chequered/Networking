using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PopupPanel : MonoBehaviour {

    [SerializeField] private Text messageTitle;
    [SerializeField] private Text messageContent;

    private CanvasGroup m_canvasGroup;

    private void Start()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        DontDestroyOnLoad(gameObject);
    }

    public void Message(string title, string content)
    {
        messageTitle.text = title;
        messageContent.text = content;
    }

	public void ClosePanel()
    {
        m_canvasGroup.interactable = false;
        m_canvasGroup.blocksRaycasts = false;
        m_canvasGroup.alpha = 0;
    }

    public void OpenPanel()
    {
        m_canvasGroup.interactable = true;
        m_canvasGroup.blocksRaycasts = true;
        m_canvasGroup.alpha = 1;
    }
}
