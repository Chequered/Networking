using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ServerPanel : MonoBehaviour {

    [SerializeField] private Text m_playerCount;
    [SerializeField] private Text m_serverName;
    [SerializeField] private Text m_serverPing;

    private HostData m_host;

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
        m_host = data;
    }

    public void Join()
    {
        GameObject.FindGameObjectWithTag("ServerList").GetComponent<ServerList>().SetPanelToWaiting(this);
        Network.Connect(m_host);
    }

    public void StartConnection()
    {
        m_serverResponseTimeout = Time.time + 2f;
        StartCoroutine(ConnectToGame());
    }

    private int m_serverResponse = 0;
    private float m_serverResponseTimeout = 2f;
    private IEnumerator ConnectToGame()
    {
        GameObject.FindGameObjectWithTag("Managers").GetComponent<NetworkView>().RPC("GameState", RPCMode.Server);

        while (Time.time < m_serverResponseTimeout)
        {
            if (m_serverResponse == 1)
            {
                //succes - lobby   
                GameObject.Find("Server Lobby Panel").GetComponent<ServerLobbyPanel>().JoinAsClient();
                m_serverResponseTimeout = 0f;
            }
            else if (m_serverResponseTimeout == 2)
            {
                //succes - inSession
                SceneManager.Instance.BuildScene();                
                m_serverResponseTimeout = 0f;
            }
            else if (m_serverResponse == 3)
            {
                CanvasManager.Instance.PopUp("Server full", "This Server has reached it's maximun amount of players, please wait and try again or try a different server.");
            }
            else if(m_serverResponse > 3)
            {
                CanvasManager.Instance.PopUp("Unkown Error", "An unknown error occured while trying to connect to the server.");
            }
            yield return new WaitForEndOfFrame();
        }
    }

    [RPC]
    public void ReceiveResponce(int responce)
    {
        m_serverResponse = responce;
    }

}
