﻿using UnityEngine;
using System.Collections;

public class TeamData {

    public const int TEAM_STARTING_RESOURCES = 35;

    private Player[] m_players;
    private int m_amountOfMembers;
    private Team m_team;
    private int m_resources;

    public TeamData(Team teamColor) {
        m_players = new Player[4];
        m_team = teamColor;
        AddResources(TEAM_STARTING_RESOURCES);
    }

    public void OnServerEvent()
    {

    }

    public void AddNewPlayer(Player player, int id)
    {
        if(m_amountOfMembers + 1 > 4)
        {
            CanvasManager.Instance.PopUp("Failed to Join", "The team you are trying to join is full, please join another team.");
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

    public int PlayerCount
    {
        get
        {
            return m_amountOfMembers;
        }
    }

    public int Resources
    {
        get
        {
            return m_resources;
        }
    }

    public void TakeResources(int resourcesToTake)
    {
        m_resources -= resourcesToTake;
        if(m_resources < 0)
        {
            m_resources = 0;
        }
        UpdateResourceUI();
    }

    public void AddResources(int resourcesToAdd)
    {
        m_resources += resourcesToAdd;
        UpdateResourceUI();
    }

    private void UpdateResourceUI()
    {
        CanvasManager.Instance.UpdateResourceUI(m_resources, TeamIDByColor(m_team));
        CanvasManager.Instance.GetComponent<NetworkView>().RPC("UpdateResourceUI", RPCMode.AllBuffered, m_resources, TeamIDByColor(m_team));
    }

    public static int TeamIDByColor(Team color)
    {
        switch (color)
	    {
            case Team.Purple:
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
                return Team.Purple;
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
            case "Purple":
                return Team.Purple;
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
