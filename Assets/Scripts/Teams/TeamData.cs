using UnityEngine;
using System.Collections;

public class TeamData {

    private Player[] m_players;
    private int m_amountOfMembers;
    private Team m_team;

    public TeamData(Team teamColor) {
        m_players = new Player[4];
        m_team = teamColor;
    }

    public void AddNewPlayer(Player player)
    {
        if(m_amountOfMembers + 1 > 4)
        {
            Debug.Log("your are try to join a full team");
            return;
        }
        else
        {
            m_players[m_amountOfMembers + 1] = player;
        }
    }

    public void RemovePlayer(Player player)
    {
        m_players[System.Array.IndexOf(m_players, player)] = null;
    }

    public Player[] Players
    {
        get
        {
            return m_players;
        }
    }

    public Team Color
    {
        get
        {
            return m_team;
        }
    }
}
