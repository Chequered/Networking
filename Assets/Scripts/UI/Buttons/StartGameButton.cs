using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartGameButton : MonoBehaviour {

	private void Start()
    {
        if(!Network.isServer)
        {
            DisableButton();
        }
    }

    public void DisableButton()
    {
        GetComponent<Button>().interactable = false;
    }

    public void EnableButton()
    {
        GetComponent<Button>().interactable = true;
    }
}
