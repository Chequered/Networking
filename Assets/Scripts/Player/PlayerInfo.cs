using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

    public Text userNameText;

    private string m_username; 

    [RPC]
	public void SetPlayerName(string playerName)
    {
        m_username = playerName;
        userNameText.text = m_username;
    }
}
