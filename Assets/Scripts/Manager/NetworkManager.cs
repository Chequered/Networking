using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public static HostData[] hostData;
    public static bool isRefreshing;
    public static string clientPlayerName = "Unnamed";

    private const string REGISTERED_GAME_NAME = "BasedGame-Official";
    private const int MAX_CLIENTS = 15;
    private const string IP = "127.0.0.1";
    private const int PORT = 23466;

    private static float refreshRequestLength = 1.25f;

    private void Start()
    {
        //MasterServer.ipAddress = IP;
        //MasterServer.port = PORT;
        //Network.natFacilitatorIP = IP;
        //Network.natFacilitatorPort = 50005;
    }

	public static void StartGame()
    {
        SceneManager.Instance.BuildScene();
    }

    public static void StartLobby(string lobbyName, string description, string password)
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

    private void OnConnectedToServer()
    {
        Debug.Log("Connected to the server!");
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

    public void RefreshAndJoin()
    {
        StartCoroutine(RefreshHostList(true));
    }

    public static IEnumerator RefreshHostList(bool joinLatest = false)
    {
        Debug.Log("Refreshing..");
        isRefreshing = true;

        MasterServer.RequestHostList(REGISTERED_GAME_NAME);

        float timeStarted = Time.time;
        float timeEnd = Time.time + refreshRequestLength;

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

    public static void UnRegisterGame()
    {
        if (Network.isServer)
        {
            Network.Disconnect(300);
            MasterServer.UnregisterHost();
        }
        if (Network.isClient)
            Network.Disconnect(300);
    }

    private void OnApplicationQuit()
    {
        NetworkManager.UnRegisterGame();
    }
}
