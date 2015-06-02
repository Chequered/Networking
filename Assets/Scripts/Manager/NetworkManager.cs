using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public static HostData[] hostData;

    private const string REGISTERED_GAME_NAME = "MainserverName-0";
    private const int MAX_PLAYERS = 16;

    private static bool isRefreshing = false;
    private static float refreshRequestLength = 3f;

    private void Start()
    {
        gameObject.AddComponent<BuildingManager>();
        gameObject.AddComponent<GridManager>();
    }

	public static void StartServer()
    {
        Network.InitializeServer(MAX_PLAYERS, 25002, false);
        MasterServer.RegisterHost(REGISTERED_GAME_NAME, "Master test server", "comment");
    }

    private void OnServerInitialized()
    {
        Debug.Log("Server has been initialized");
    }

    private void OnMasterServerEvent(MasterServerEvent mEvent)
    {
        if(mEvent == MasterServerEvent.RegistrationSucceeded)
        {
            Debug.Log("Registration succesful!");
        }
    }

    public static IEnumerator RefreshHostList()
    {
        Debug.Log("Refreshing..");

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
