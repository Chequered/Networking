﻿using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;

    [HideInInspector] public HostData[] hostData;
    [HideInInspector] public bool isRefreshing;
    [HideInInspector] public string clientPlayerName = "Unnamed Dwarf";
    [HideInInspector] public int clientTeamID;
    [HideInInspector] public int clientPlayerID;

	[SerializeField] private bool useOfficialMasterserver;
    private const string REGISTERED_GAME_NAME = "BasedGame-Official";
    private const int MAX_CLIENTS = 15;
    private const string IP = "172.17.59.116";
    private const int PORT = 23466;

    private float m_refreshRequestLength = 1.25f;
    private NetworkView m_networkView;

    private void Start()
    {
        Instance = this;
        m_networkView = GetComponent<NetworkView>();

		if(!useOfficialMasterserver)
		{
			MasterServer.ipAddress = IP;
			MasterServer.port = PORT;
		}
    }

	public void StartGame()
    {
        SceneManager.Instance.BuildScene();
        m_networkView.RPC("OnStartGame", RPCMode.AllBuffered);
    }

    [RPC]
    public void OnStartGame()
    {
        if (!Network.isServer)
            SceneManager.Instance.BuildScene();
        if (Network.isServer)
        {
            gameObject.GetComponent<ServerMaster>().StartGame();
        }
    }

    public void StartLobby(string lobbyName, string description, string password)
    {
        Network.incomingPassword = password;
        Network.InitializeServer(MAX_CLIENTS, 25002, false);
        MasterServer.RegisterHost(REGISTERED_GAME_NAME, lobbyName, description);
    }

    private void OnServerInitialized()
    {
        Debug.Log("Server has been initialized");
    }

    private void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("Player connected from: " + player.ipAddress);
    }

    private void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.LogError(error);
    }

    private void OnMasterServerEvent(MasterServerEvent mEvent)
    {
        if(mEvent == MasterServerEvent.RegistrationSucceeded)
        {
            Debug.Log("Registration succesful!");
        }
    }

    public void Refresh()
    {
        StartCoroutine(RefreshHostList(true));
    }

    public IEnumerator RefreshHostList(bool joinLatest = false)
    {
        Debug.Log("Refreshing..");
        isRefreshing = true;

        MasterServer.RequestHostList(REGISTERED_GAME_NAME);

        float timeStarted = Time.time;
        float timeEnd = Time.time + m_refreshRequestLength;

        while(Time.time < timeEnd)
        {
            hostData = MasterServer.PollHostList();
            yield return new WaitForEndOfFrame();
        }

        if (hostData == null || hostData.Length == 0)
        {
            Debug.Log("No active servers has been found");
        }
        Debug.Log(hostData.Length + " servers found.");
        isRefreshing = false;

        if(joinLatest)
        {
            SceneManager.Instance.JoinGame(hostData[hostData.Length - 1]);
        }
    }

    public void UnRegisterGame()
    {
        if (Network.isServer)
        {
            MasterServer.UnregisterHost();
            MasterServer.ClearHostList();
        }
    }

    private void OnApplicationQuit()
    {
        UnRegisterGame();
    }
}
