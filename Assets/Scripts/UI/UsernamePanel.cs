using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UsernamePanel : MonoBehaviour {

    public InputField usernameInputField; 

    private void Start()
    {
        if(PlayerPrefs.HasKey("Username"))
        {
            NetworkManager.Instance.clientPlayerName = PlayerPrefs.GetString("Username");
            usernameInputField.text = PlayerPrefs.GetString("Username");
        }
    }

	public void SaveUsername()
    {
        NetworkManager.Instance.clientPlayerName = usernameInputField.text;
        PlayerPrefs.SetString("Username", usernameInputField.text);
    }
}
