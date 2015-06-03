using UnityEngine;
using System.Collections;

public class GameLobby : MonoBehaviour {

    private TeamData[] m_teams;

    public GameLobby()
    {
        m_teams = new TeamData[4];

        m_teams[0] = new TeamData(Team.Blue);
        m_teams[1] = new TeamData(Team.Red);
        m_teams[2] = new TeamData(Team.Yellow);
        m_teams[3] = new TeamData(Team.Green);
    }

    public TeamData[] Teams
    {
        get
        {
            return m_teams;
        }
    }

    public void JoinTeam(Team color, Player player)
    {
        foreach (TeamData team in m_teams)
        {
            if(team.Color == color)
            {
                team.AddNewPlayer(player);
            }
        }
    }
}
