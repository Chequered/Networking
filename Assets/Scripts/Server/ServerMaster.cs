using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerMaster : MonoBehaviour {

    public static ServerMaster Instance;

    [HideInInspector] public bool isInSession = false;

    private GameLobby m_lobby;
    private bool m_isClient;

    private float eventCooldown;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        isInSession = true;
        eventCooldown = Time.time + 5;
    }

    private void Update()
    {
        if(!m_isClient && isInSession)
        {
            if(Time.time > eventCooldown)
            {
                m_lobby.OnServerEvent();
                eventCooldown = Time.time + 5;
            }
        }
    }

    [RPC]
    public void StartCapture(int unqiueBuildingID, int teamID)
    {
        List<Building> buildings = BuildingManager.Instance.GetBuildings();
        if(buildings[unqiueBuildingID].ProgressCapture(teamID) >= 100)
        {
            buildings[unqiueBuildingID].ChangeTeam(teamID);
        }
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

    public void SetAsClient()
    {
        m_isClient = true;
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
