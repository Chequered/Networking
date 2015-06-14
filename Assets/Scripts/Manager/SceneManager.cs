using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public static SceneManager Instance;

    public GameLobby gameLobby;

    private GameObject m_playerObject;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Application.LoadLevel(0);
        } 
    }

	public void JoinGame(HostData server)
    {
        Network.Connect(server);
        BuildScene();
    }

    public void BuildScene()
    {
        SpawnPlayer();
        CanvasManager.Instance.CloseAllMenus();
        CanvasManager.Instance.ShowGameUI();
        GridManager.Instance.GenerateGrid();
        BuildingManager.Instance.BuildStartingBuildings();
        if(Network.isServer)
        {
            ServerMaster.Instance.isInSession = true;
        }
    }

    private void SpawnPlayer()
    {
        m_playerObject = Network.Instantiate(Resources.Load("Player/Player " + TeamData.TeamColorByID(NetworkManager.Instance.clientTeamID).ToString()) as GameObject, Vector2.zero, Quaternion.identity, 0) as GameObject;
        m_playerObject.GetComponent<NetworkView>().RPC("SetPlayerName", RPCMode.AllBuffered, NetworkManager.Instance.clientPlayerName);
        m_playerObject.GetComponent<PlayerInfo>().SetPlayerName(NetworkManager.Instance.clientPlayerName);
        m_playerObject.GetComponent<NetworkView>().RPC("SetTeam", RPCMode.AllBuffered, NetworkManager.Instance.clientTeamID);
        m_playerObject.GetComponent<PlayerInfo>().SetTeam(NetworkManager.Instance.clientTeamID);
        m_playerObject.GetComponent<NetworkView>().RPC("SetStats", RPCMode.AllBuffered, Player.STARTING_DAMAGE);
        m_playerObject.GetComponent<PlayerInfo>().SetStats(Player.STARTING_DAMAGE);
    }

    private IEnumerator WaitForRespawnTimer()
    {
        yield return new WaitForSeconds(10);
        SpawnPlayer();
    }

    public void Respawn()
    {
        StartCoroutine(WaitForRespawnTimer());
    }

    private void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        CanvasManager.Instance.PopUp("Lost Connection", "Connection to the game has been lost. Either the host closed the server or there is something wrong with your internet connection");
        Application.LoadLevel(0);
    }
}
