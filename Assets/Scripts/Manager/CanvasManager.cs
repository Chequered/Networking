using UnityEngine;
using System.Collections;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager Instance;

    private void Start()
    {
        Instance = this;
    }

    public GameObject pauseMenu;
    public GameObject serverList;
    public GameObject serverLobby;

    public void CloseAllMenus()
    {
        serverList.GetComponent<ServerList>().BackToMain();
        serverLobby.GetComponent<ServerLobbyPanel>().TogglePanel("Close");
        pauseMenu.GetComponent<PauseMenuActions>().ResumeGame();
    }
}
