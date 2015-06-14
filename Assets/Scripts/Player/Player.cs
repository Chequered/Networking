using UnityEngine;
using System.Collections;

public enum Team
{
    None,
    Purple,
    Red,
    Yellow,
    Green
}

public class Player {

    public const int STARTING_DAMAGE = 12;

    private string m_name;
    private Team m_team;
    private int m_damage;
    private int m_resources;
    private NetworkPlayer m_playerData;

    public Player(string name, Team team, NetworkPlayer playerData)
    {
        m_name = name;
        m_team = team;
        m_damage = STARTING_DAMAGE;
        m_playerData = playerData;
    }

    public string Name
    {
        get
        {
            return m_name;
        }
    }

    public Team Team
    {
        get
        {
            return m_team;
        }
    }

    public int Damage
    {
        get
        {
            return m_damage;
        }
    }

    public int Resources
    {
        get
        {
            return m_resources;
        }
    }

    public NetworkPlayer PlayerData
    {
        get
        {
            return m_playerData;
        }
    }
}
