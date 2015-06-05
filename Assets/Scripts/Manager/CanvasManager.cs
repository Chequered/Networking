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

    public void CloseAllMenus()
    {
        serverList.GetComponent<ServerList>().BackToMain();
        pauseMenu.GetComponent<PauseMenuActions>().ResumeGame();
    }
}
