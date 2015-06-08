using UnityEngine;
using System.Collections;

public enum BuildingType
{
    HeadQuarters,
    Wall,
    Turret,
    Cannon,
    Resource
}

public class Building : MonoBehaviour
{

    private Team m_team;
    private BuildingType m_type;
    private int m_x;
    private int m_y;
    private int m_hp;
    private int m_size;

    public Building(int x, int y, int hp)
    {
        m_x = x;
        m_y = y;
        m_hp = hp;
    }

    public void Build(Team team, BuildingType type)
    {
        m_type = type;
        m_size = SizeByType(type);
        m_team = team;
    }

    public void DealDamage(int dmg)
    {
        if (m_hp - dmg <= 0)
        {
            //BuildingManager.RemoveBuilding(this);
        }
        else
        {
            m_hp -= dmg;
            //Update UI;
        }
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

    public int HP
    {
        get
        {
            return m_hp;

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
            case BuildingType.Cannon:
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
                return BuildingType.Cannon;
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
            case BuildingType.Cannon:
                return 4;
            case BuildingType.HeadQuarters:
                return 5;
            default:
                return 1;
        }
    }
}
