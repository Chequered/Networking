using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

    public static SceneManager Instance;

    public GameLobby gameLobby;

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
        CanvasManager.Instance.CloseAllMenus();
        GridManager.Instance.GenerateGrid();
    }
}
