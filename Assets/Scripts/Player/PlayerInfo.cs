using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerInfo : MonoBehaviour {

    public Text userNameText;

    private string m_username;
    private int m_teamID;
    private NetworkView m_networkView;
    private int m_damage;

    private void Start()
    {
        m_networkView = GetComponent<NetworkView>();
    }

    [RPC]
	public void SetPlayerName(string playerName)
    {
        m_username = playerName;
        userNameText.text = m_username;
    }

    [RPC]
    public void SetTeam( int teamID)
    {
        m_teamID = teamID;
    }

    [RPC]
    public void SetStats(int dmg)
    {
        m_damage = dmg;
    }

    public int TeamID
    {
        get
        {
            return m_teamID;
        }
    }

    public int Damage
    {
        get
        {
            return m_damage;
        }
    }

    public void StartCapture(BuildingInfo building)
    {
        if(m_networkView.isMine)
        {
            if(Network.isServer)
            {
                ServerMaster.Instance.StartCapture(building.UniqueID, NetworkManager.Instance.clientTeamID);
            }
            else
            {
                ServerMaster.Instance.GetComponent<NetworkView>().RPC("StartCapture", RPCMode.Server, building.UniqueID, NetworkManager.Instance.clientTeamID);
            }
        }
    }
}
