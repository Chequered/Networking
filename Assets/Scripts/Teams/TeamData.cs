using UnityEngine;
using System.Collections;

public class TeamData : MonoBehaviour {

    private Player[] m_players;

    private void Start()
    {
        m_players = new Player[4];
    }
}
