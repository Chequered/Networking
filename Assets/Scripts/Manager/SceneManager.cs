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
        GridManager.Instance.GenerateGrid();
    }

    private void SpawnPlayer()
    {
        m_playerObject = Network.Instantiate(Resources.Load("Player/PlayerPlaceholder") as GameObject, Vector2.zero, Quaternion.identity, 0) as GameObject;
        m_playerObject.AddComponent<MousePositionTracker>();
        m_playerObject.GetComponent<NetworkView>().RPC("SetPlayerName", RPCMode.AllBuffered, NetworkManager.Instance.clientPlayerName);
        m_playerObject.GetComponent<PlayerInfo>().SetPlayerName(NetworkManager.Instance.clientPlayerName);
    }
    
}
