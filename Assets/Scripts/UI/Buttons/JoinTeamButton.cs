using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JoinTeamButton : MonoBehaviour {

    private int m_teamID;
    private Team m_team;
    private ServerLobbyPanel m_panel;
    private bool m_taken = false;

    private Text m_buttonContent;

    private void Start()
    {
        System.Int32.TryParse(transform.name, out m_teamID);
        m_team = TeamData.TeamColorByString(transform.parent.name);
        m_panel = GameObject.Find("Server Lobby Panel").GetComponent<ServerLobbyPanel>();
        m_buttonContent = transform.FindChild("Name").GetComponent<Text>();
    }

    public void JoinSlot()
    {
        if(!m_taken)
        {
            m_panel.JoinTeam(this, m_teamID, m_team, NetworkManager.Instance.clientPlayerName);
            m_taken = true;
        }
    }

    public void UpdateButton(string newPlayerName)
    {
        m_taken = true;
        GetComponent<Button>().interactable = false;
        transform.FindChild("Name").GetComponent<Text>().text = newPlayerName;
        transform.FindChild("Name").GetComponent<Text>().fontStyle = FontStyle.Bold;
    }

    [RPC]
    public void ResetButton()
    {
        m_taken = false;
        GetComponent<Button>().interactable = true;
        m_buttonContent.text = "Empty";
        m_buttonContent.fontStyle = FontStyle.Italic;
    }

    private void OnApplicationExit()
    {
        ResetButton();
        GetComponent<NetworkView>().RPC("ResetButton", RPCMode.AllBuffered);
    }

    public Team oldTeam
    {
        get
        {
            return m_team;
        }
    }

    public int oldTeamId
    {
        get
        {
            return m_teamID;
        }
    }
    
}

