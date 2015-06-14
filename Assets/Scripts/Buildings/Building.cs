using UnityEngine;
using System.Collections;

public enum BuildingType
{
    HeadQuarters,
    Wall,
    Turret,
    Drill,
    Resource
}

public class Building {

    private Team m_team;
    private BuildingType m_type;
    private int m_x;
    private int m_y;
    private int m_size;
    private GameObject m_gameObject;
    private int[] m_captureProgress;
    private int m_hitPoints;

    public Building(int x, int y)
    {
        m_x = x;
        m_y = y;
        m_captureProgress = new int[4];
    }

    public void Build(Team team, BuildingType type, GameObject gameObject)
    {
        m_type = type;
        m_size = SizeByType(type);
        m_team = team;
        m_gameObject = gameObject;
        m_hitPoints = HPByType(m_type);
    }

    public int ProgressCapture(int teamID)
    {
        for (int i = 0; i < m_captureProgress.Length; i++)
        {
            if(i != teamID - 1)
            {
                m_captureProgress[i] = 0;
            }
        }
        m_captureProgress[teamID - 1] += 20;
        Mathf.Clamp(m_captureProgress[teamID - 1], 0, 100);
        return m_captureProgress[teamID - 1];
    }

    public void ChangeTeam(int newTeamID)
    {
        m_team = TeamData.TeamColorByID(newTeamID);
        m_gameObject.GetComponent<BuildingInfo>().ChangeTeamGraphics(newTeamID);
        m_gameObject.GetComponent<NetworkView>().RPC("ChangeTeamGraphics", RPCMode.AllBuffered, newTeamID);
    }

    public bool DoDamage(int dmg)
    {
        m_hitPoints -= dmg;
        if(m_hitPoints <= 0)
        {
            return true;
        }
        return false;
    }

    #region getters

    public Team Team
    {
        get
        {
            return m_team;
        }
    }

    public int X
    {
        get
        {
            return m_x;
        }
    }

    public int Y
    {
        get
        {
            return m_y;
        }
    }

    public BuildingType Type
    {
        get
        {
            return m_type;
        }
    }

    public int Size
    {
        get
        {
            return m_size;
        }
    }

    public GameObject GameObject
    {
        get
        {
            return m_gameObject;
        }
    }

    private int HPByType(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.HeadQuarters:
                return 500;
            case BuildingType.Wall:
                return 50;
            case BuildingType.Turret:
                return 25;
            case BuildingType.Drill:
                return 50000;
            case BuildingType.Resource:
                return 1000;
            default:
                return 50000;
        }
    }

    #endregion

    #region converters
    public static int SizeByType(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.HeadQuarters:
                return 3;
            case BuildingType.Wall:
                return 1;
            case BuildingType.Turret:
                return 1;
            case BuildingType.Drill:
                return 2;
            case BuildingType.Resource:
                return 2;
            default:
                return 1;
        }
    }

    public static BuildingType TypeById(int ID)
    {
        switch (ID)
        {
            case 1:
                return BuildingType.Wall;
            case 2:
                return BuildingType.Turret;
            case 3:
                return BuildingType.Resource;
            case 4:
                return BuildingType.Drill;
            case 5:
                return BuildingType.HeadQuarters;
            default:
                return BuildingType.Wall;
        }
    }

    public static int IdByType(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.Wall:
                return 1;
            case BuildingType.Turret:
                return 2;
            case BuildingType.Resource:
                return 3;
            case BuildingType.Drill:
                return 4;
            case BuildingType.HeadQuarters:
                return 5;
            default:
                return 1;
        }
    }

    public static BuildingType TypeByMode(BuildMode mode)
    {
        switch (mode)
	    {
            case BuildMode.Wall:
                return BuildingType.Wall;
            case BuildMode.Turret:
             return BuildingType.Turret;
            default:
             return BuildingType.Wall;
	    }
    }

    public static int CostById(int buildingID)
    {
        switch (buildingID)
        {
            case 1:
                return 10;
            case 2:
                return 35;
            case 3:
                return 0;
            case 4:
                return 0;
            case 5:
                return 0;
            default:
                return 10;
        }
    }

#endregion
}
