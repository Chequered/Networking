using UnityEngine;
using System.Collections;

public class ServerMaster : MonoBehaviour {

    public static ServerMaster Instance;
    //TODO: replace later with GameObject.FindObjectOfType

    [HideInInspector] public bool isInSession = false;

    private GameLobby m_lobby;

    public void StartGame()
    {
        isInSession = true;
    }

    public GameLobby Lobby
    {
        get
        {
            return m_lobby;
        }
        set
        {
            m_lobby = value;
        }
    }

    [RPC]
    public void GameState(NetworkMessageInfo info)
    {
        int result = 4;
        if(!isInSession && Network.connections.Length < 15)
        {
            result = 1;
        }
        else if(isInSession && Network.connections.Length < 15)
        {
            result = 2;
        }
        else if(Network.connections.Length >= 15)
        {
            result = 3;
        }
        GameObject.FindGameObjectWithTag("ServerList").GetComponent<NetworkView>().RPC("ReceiveResponce", info.sender, result);
    }
}
