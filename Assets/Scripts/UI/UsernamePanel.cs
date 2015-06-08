using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UsernamePanel : MonoBehaviour {

    public InputField usernameInputField; 

	public void SaveUsername()
    {
        NetworkManager.Instance.clientPlayerName = usernameInputField.text;
    }
}
