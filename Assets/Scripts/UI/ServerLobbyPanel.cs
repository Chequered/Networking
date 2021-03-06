﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ServerLobbyPanel : MonoBehaviour {

    public static ServerLobbyPanel Instance;

    private CanvasGroup m_serverSettingsCanvasGroup;
    private CanvasGroup m_canvasGroup;
    private CanvasGroup m_teamsCanvasGroup;
    private InputField m_serverName;
    private InputField m_serverPassword;
    private InputField m_serverDescription;

    private GameLobby m_gameLobby;
    private JoinTeamButton m_oldButton;
    private JoinTeamButton m_newButton;

    private JoinTeamButton[] m_playerSlots;
    private List<NetworkPlayer> m_networkPlayers;

    private void Start()
    {
        Instance = this;

        m_canvasGroup = GetComponent<CanvasGroup>();
        m_teamsCanvasGroup = transform.FindChild("Teams").GetComponent<CanvasGroup>();
        m_serverSettingsCanvasGroup = transform.FindChild("Server Settings").GetComponent<CanvasGroup>();
        m_serverName = m_serverSettingsCanvasGroup.transform.FindChild("Server Name").GetComponent<InputField>();
        m_serverPassword = m_serverSettingsCanvasGroup.transform.FindChild("Server Password").GetComponent<InputField>();
        m_serverDescription = m_serverSettingsCanvasGroup.transform.FindChild("Server Description").GetComponent<InputField>();

        m_playerSlots = new JoinTeamButton[16];

        m_networkPlayers = new List<NetworkPlayer>();

        if(Network.isServer)
        {
            m_networkPlayers.Add(Network.player);
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                m_playerSlots[(i * 4) + j] = m_teamsCanvasGroup.transform.FindChild("" + TeamData.TeamColorByID(i + 1)).FindChild("" + (j + 1)).GetComponent<JoinTeamButton>();
            }
        }
    }

    private void OnPlayerConnected(NetworkPlayer player)
    {
        m_networkPlayers.Add(player);
    }

    private void OnPlayerDisconnected(NetworkPlayer player)
    {
        m_networkPlayers.Remove(player);
        int[] deletedPlayerData = m_gameLobby.RemovePlayer(player);
        ResetButton(deletedPlayerData[0], deletedPlayerData[1]);
    }

    public void JoinAsClient()
    {
        GameObject.FindObjectOfType<ServerMaster>().SetAsClient();
        TogglePanel("Open");

        //enable UI
        m_serverSettingsCanvasGroup.interactable = false;
        m_teamsCanvasGroup.alpha = 1;
        m_teamsCanvasGroup.blocksRaycasts = true;
        m_teamsCanvasGroup.interactable = true;
    }

    public void StartLobby()
    {
        NetworkManager.Instance.StartLobby(m_serverName.text, m_serverDescription.text, m_serverPassword.text);
        m_teamsCanvasGroup.alpha = 1;
        m_teamsCanvasGroup.interactable = true;
        m_teamsCanvasGroup.blocksRaycasts = true;
        m_gameLobby = new GameLobby();
        transform.FindChild("Start Game Button").GetComponent<StartGameButton>().EnableButton();
    }

    public void StartGame()
    {
        if (Network.isServer)
        {
            if (ServerLobbyPanel.Instance.HasEnoughPlayers)
            {
                ServerMaster.Instance.Lobby = m_gameLobby;
                NetworkManager.Instance.StartGame();
            }
        }
    }
    
    private int m_newTeamID;
    private int m_newTeamColorID;
    private string m_newPlayerName;
    public void JoinTeam(JoinTeamButton refButton, int teamID, Team teamColor, string playerName)
    {
        if(m_newButton != null)
        {
            m_oldButton = m_newButton;
        }
        m_newButton = refButton;

        m_newTeamID = teamID;
        m_newTeamColorID = TeamData.TeamIDByColor(teamColor);
        m_newPlayerName = playerName;

        timeOut = Time.time + 2f;
        if(Network.isServer)
        {
            CheckSlot(TeamData.TeamIDByColor(teamColor), teamID);
        }
        else
        {
            GetComponent<NetworkView>().RPC("CheckSlot", RPCMode.Server, TeamData.TeamIDByColor(teamColor), teamID);
        }
        StartCoroutine(WaitForResponce());
    }

    private int m_slotAvaiable = 0;
    private float timeOut;
    private IEnumerator WaitForResponce()
    {
        while (Time.time < timeOut)
        {
            if (m_slotAvaiable == 1)
            {
                if(m_oldButton != null)
                {
                    GetComponent<NetworkView>().RPC("JoinNewSlot", 
                        RPCMode.Server,
                        m_newTeamID,
                        m_newTeamColorID,
                        m_newPlayerName);
            
                        if(Network.isServer)
                        {
                            JoinNewSlot(
                                m_newTeamID,
                                m_newTeamColorID,
                                m_newPlayerName);
                        }
                }
                else
                {
                    if (Network.isServer)
                    {
                        JoinNewSlot(
                            m_newTeamID,
                            m_newTeamColorID,
                            m_newPlayerName);
                    }
                    else
                    {
                        GetComponent<NetworkView>().RPC("JoinNewSlot",
                            RPCMode.Server,
                            m_newTeamID,
                            m_newTeamColorID,
                            m_newPlayerName);
                    }
                }
                NetworkManager.Instance.clientPlayerID = m_newTeamID;
                NetworkManager.Instance.clientTeamID = m_newTeamColorID;
                timeOut = 0f;
            }else if(m_slotAvaiable == 2)
            {
                Debug.Log("slot is full!");
            }
            yield return new WaitForEndOfFrame();
        }
    }

    [RPC]
    public void CheckSlot(int ColorID, int teamID, NetworkMessageInfo info)
    {
        if(Network.isServer)
        {
            if (m_gameLobby.SlotAvaiable(TeamData.TeamColorByID(ColorID), teamID))
            {
                GetComponent<NetworkView>().RPC("ReceiveResponce", info.sender, 1);
                if(m_oldButton != null)
                {
                    Team team = TeamData.TeamColorByID(teamID);
                    m_gameLobby.LeaveTeam(m_oldButton.oldTeam, m_oldButton.oldTeamId);
                }
            }
            else
            {
                GetComponent<NetworkView>().RPC("ReceiveResponce", info.sender, 2);
            }
        }
    }

    public void CheckSlot(int ColorID, int teamID)
    {
        if (Network.isServer)
        {
            if (m_gameLobby.SlotAvaiable(TeamData.TeamColorByID(ColorID), teamID))
            {
                m_slotAvaiable = 1;
            }
            else
            {
                m_slotAvaiable = 2;
            }
        }
    }

    [RPC]
    public void ReceiveResponce(int result)
    {
        m_slotAvaiable = result;
    }

    [RPC]
    public void JoinNewSlot(int teamID, int colorID, string playerName, NetworkMessageInfo info = new NetworkMessageInfo())
    {
        Team team = TeamData.TeamColorByID(teamID);
        if(Network.isServer)
        {
            NetworkPlayer result = new NetworkPlayer();
            if(info.sender.ipAddress != Network.player.ipAddress)
            {
                foreach (NetworkPlayer player in m_networkPlayers)
                {
                    if (player.ipAddress == info.sender.ipAddress)
                    {
                        result = player;
                    }
                }
            }
            else
            {
                result = Network.player;
            }
            m_gameLobby.JoinTeam(team, new Player(playerName, team, result), teamID);
            UpdateTeamSlot(colorID, teamID, playerName, false);
        }
        ResetOldButton();
        GetComponent<NetworkView>().RPC("ResetOldButton", RPCMode.AllBuffered);
        GetComponent<NetworkView>().RPC("UpdateTeamSlot", RPCMode.AllBuffered, colorID, teamID, playerName, false);
    }

    [RPC]
    public void ResetOldButton()
    {
        if (m_oldButton != null)
        {
            m_playerSlots[System.Array.IndexOf(m_playerSlots, m_oldButton)].GetComponent<NetworkView>().RPC("ResetButton", RPCMode.AllBuffered);
            m_playerSlots[System.Array.IndexOf(m_playerSlots, m_oldButton)].GetComponent<JoinTeamButton>().ResetButton();
        }
    }

    [RPC]
    public void ResetButton(int teamID, int playerID)
    {
        if(!ServerMaster.Instance.isInSession)
        {
            transform.FindChild("Teams").FindChild("" + TeamData.TeamColorByID(teamID)).FindChild("" + playerID).GetComponent<NetworkView>().RPC("ResetButton", RPCMode.AllBuffered);
            transform.FindChild("Teams").FindChild("" + TeamData.TeamColorByID(teamID)).FindChild("" + playerID).GetComponent<JoinTeamButton>().ResetButton();
        }
    }

    [RPC]
    public void UpdateTeamSlot(int colorID, int teamID, string playerName, bool isMe)
    {
        transform.FindChild("Teams").FindChild("" + TeamData.TeamColorByID(colorID)).FindChild("" + teamID).GetComponent<JoinTeamButton>().UpdateButton(playerName);
    }

    public void TogglePanel(string way)
    {
        if(way == "Open")
        {
            m_canvasGroup.alpha = 1;
            m_canvasGroup.interactable = true;
            m_canvasGroup.blocksRaycasts = true;
        }
        else if (way == "Close")
        {
            m_canvasGroup.alpha = 0;
            m_canvasGroup.interactable = false;
            m_canvasGroup.blocksRaycasts = false;
        }
    }

    public void ReturnToServerList()
    {
        NetworkManager.Instance.UnRegisterGame();
        TogglePanel("Close");
        CanvasManager.Instance.serverList.GetComponent<ServerList>().OpenServerList();
    }

    public bool HasEnoughPlayers
    {
        get
        {
            if(m_gameLobby.TeamHavePlayers() >= 1)
            {
                return true;
            }
            return false;
        }
    }
}
