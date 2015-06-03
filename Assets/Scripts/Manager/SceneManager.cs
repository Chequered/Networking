using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public static void JoinGame(HostData server)
    {
        Debug.Log("Joined server");
        Network.Connect(server);
        CanvasManager.Instance.CloseAllMenus();
        GridManager.Instance.GenerateGrid();
    }
}
