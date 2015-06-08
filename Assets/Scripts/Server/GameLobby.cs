using UnityEngine;
using System.Collections;

public class GameLobby {

    private TeamData[] m_teams;
    private int[] m_teamResources;

    public GameLobby()
    {
        m_teams = new TeamData[4];
        m_teamResources = new int[4];

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

    public void JoinTeam(Team teamColor, Player player, int id)
    {
        foreach (TeamData team in m_teams)
        {
            if (team.Color == teamColor)
            {
                team.AddNewPlayer(player, id);
            }
        }
    }

    public void LeaveTeam(Team teamColor, int teamID)
    {
        foreach (TeamData team in m_teams)
        {
            if (team.Color == teamColor)
            {
                team.RemovePlayer(teamID);
            }
        }
    }

    public bool SlotAvaiable(Team teamColor , int teamID)
    {
        foreach (TeamData team in m_teams)
        {
            if (team.Color == teamColor)
            {
                if (team.Players[teamID - 1] == null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public int TeamHavePlayers()
    {
        int result = 0;
        foreach (TeamData team in m_teams)
        {
            if (team.PlayerCount > 0)
            {
                result++;
            }
        }
        return result;
    }

    public TeamData GetTeam(Team teamColor)
    {
        foreach (TeamData team in m_teams)
        {
            if (team.Color == teamColor)
            {
                return team;
            }
        }
        return null;
    }
}
