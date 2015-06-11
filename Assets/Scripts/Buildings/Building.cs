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

    public Building(int x, int y)
    {
        m_x = x;
        m_y = y;
    }

    public void Build(Team team, BuildingType type, GameObject gameObject)
    {
        m_type = type;
        m_size = SizeByType(type);
        m_team = team;
        m_gameObject = gameObject;
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

    #endregion

    public static int SizeByType(BuildingType type)
    {
        switch (type)
        {
            case BuildingType.HeadQuarters:
                return 3;
                break;
            case BuildingType.Wall:
                return 1;
                break;
            case BuildingType.Turret:
                return 1;
                break;
            case BuildingType.Drill:
                return 2;
                break;
            case BuildingType.Resource:
                return 2;
                break;
            default:
                return 1;
                break;
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
}
