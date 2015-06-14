using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour {

    [SerializeField] private Text m_resourceValueText;
    private CanvasGroup m_canvasGroup;

    private void Start()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

	public void UpdateResources(int newAmount)
    {
        m_resourceValueText.text = newAmount.ToString();
    }

    public void Show()
    {
        m_canvasGroup.alpha = 1;
        m_canvasGroup.interactable = true;
        m_canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        m_canvasGroup.alpha = 0;
        m_canvasGroup.interactable = false;
        m_canvasGroup.blocksRaycasts = false;
    }
}
