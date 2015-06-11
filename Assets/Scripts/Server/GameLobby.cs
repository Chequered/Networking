using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLobby {

    private TeamData[] m_teams;
    private int[] m_teamResources;

    private int m_kickID;

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

    public int[] RemovePlayer(NetworkPlayer player)
    {
        int[] result = new int[2];
        for (int i = 0; i < m_teams.Length; i++)
        {
            for (int j = 0; j < m_teams[i].Players.Length; j++)
            {
                Player p = m_teams[i].Players[j];

                if(p != null)
                {
                    if (p.PlayerData == player)
                    {
                        result[0] = TeamData.TeamIDByColor(p.Team);
                        result[1] = System.Array.IndexOf(m_teams[i].Players, p) + 1;
                        m_kickID = j;

                        break;
                    }

                }
            }
            m_teams[i].Players[m_kickID] = null;
        }
        return result;
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
