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

    public void AddNewPlayer(Player player, int id)
    {
        if(m_amountOfMembers + 1 > 4)
        {
            Debug.Log("your are try to join a full team");
            return;
        }
        else
        {
            m_players[id - 1] = player;
            m_amountOfMembers++;
        }
    }

    public void RemovePlayer(int teamID)
    {
        m_players[teamID - 1] = null;
        m_amountOfMembers--;
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

    public static int TeamIDByColor(Team color)
    {
        switch (color)
	    {
            case Team.Blue:
                return 1;
            case Team.Red:
                return 2;
            case Team.Yellow:
                return 3;
            case Team.Green:
                return 4;
		    default:
                return 0;
	    }
    }

    public static Team TeamColorByID(int ID)
    {
        switch (ID)
	    {
            case 1:
                return Team.Blue;
            case 2:
                return Team.Red;
            case 3:
                return Team.Yellow;
            case 4:
                return Team.Green;
		    default:
                return Team.None;
	    }
    }

    public static Team TeamColorByString(string teamName)
    {
        switch (teamName)
        {
            case "Blue":
                return Team.Blue;
            case "Red":
                return Team.Red;
            case "Yellow":
                return Team.Yellow;
            case "Green":
                return Team.Green;
            default:
                return Team.None;
        }
    }
}
