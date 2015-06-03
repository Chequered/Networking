using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerPanel : MonoBehaviour {

    [SerializeField] private Text m_playerCount;
    [SerializeField] private Text m_serverName;
    [SerializeField] private Text m_serverPing;

    private HostData host;

    public void SetPosition(int index)
    {
        Vector3 newPos = new Vector3(0, 0, 0);
        newPos.y = ((GetComponent<RectTransform>().sizeDelta.y * index) + (5 * index)) * -1;
        GetComponent<RectTransform>().anchoredPosition = newPos;
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    public void SetServer(HostData data)
    {
        m_playerCount.text = data.connectedPlayers + "/" + data.playerLimit;
        m_serverName.text = data.gameName;
        m_serverPing.text = "0";
    }

    public void Join()
    {
        SceneManager.JoinGame(host);
    }

}
