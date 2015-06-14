using UnityEngine;
using System.Collections;

public class CanvasManager : MonoBehaviour {

    public static CanvasManager Instance;

    [SerializeField] private PopupPanel popupPanel;

    private void Start()
    {
        Instance = this;
    }

    public GameObject pauseMenu;
    public GameObject serverList;
    public GameObject serverLobby;
    public GameUIManager gameUI;

    public void CloseAllMenus()
    {
        serverList.GetComponent<ServerList>().BackToMain();
        serverLobby.GetComponent<ServerLobbyPanel>().TogglePanel("Close");
        pauseMenu.GetComponent<PauseMenuActions>().ResumeGame();
    }

    public void PopUp(string title, string message)
    {
        popupPanel.OpenPanel();
        popupPanel.Message(title, message);
    }

    [RPC]
    public void UpdateResourceUI(int resources, int teamID)
    {
        if(NetworkManager.Instance.clientTeamID == teamID)
            gameUI.UpdateResources(resources);
    }

    public void ShowGameUI()
    {
        gameUI.Show();
    }
}
