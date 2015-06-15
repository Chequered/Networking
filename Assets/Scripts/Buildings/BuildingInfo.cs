using UnityEngine;
using System.Collections;

public class BuildingInfo : MonoBehaviour {

    private int m_gridX;
    private int m_gridY;
    private int m_typeID;
    private int m_uniqueID;
    private Building m_data;

    public int UniqueID
    {
        get
        {
            return m_uniqueID;
        }
    }

    [RPC]
    public void ChangeTeamGraphics(int newTeamID)
    {
        //change graphics
        Debug.Log("Changed team to:" + TeamData.TeamColorByID(newTeamID));
        switch (TeamData.TeamColorByID(newTeamID))
        {
            case Team.None:
                break;
            case Team.Purple:
        GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            case Team.Red:
        GetComponent<SpriteRenderer>().color = Color.red;
                break;
            case Team.Yellow:
        GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case Team.Green:
        GetComponent<SpriteRenderer>().color = Color.green;
                break;
            default:
                break;
        }
    }

    [RPC]
    public void SetInfo(int x, int y, int buildingTypeID, int uniqueID, Building data)
    {
        m_gridX = x;
        m_gridY = y;
        m_typeID = buildingTypeID;
        m_uniqueID = uniqueID;
        m_data = data;
    }

    [RPC]
    private void SyncBuilding(int x, int y, int buildingID, int teamID)
    {
        Building building = new Building(x, y);
        building.Build(TeamData.TeamColorByID(teamID), Building.TypeById(buildingID), this.gameObject);

        BuildingManager.Instance.AddBuilding(building);
        GridManager.Instance.RegisterBuilding(building.X, building.Y, building.Size, TeamData.TeamIDByColor(building.Team));
        SetInfo(x, y, buildingID, BuildingManager.Instance.GetBuildings().Count - 1, building);

        if(teamID != NetworkManager.Instance.clientTeamID)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    public void Attack(int dmg)
    {
        if(m_data.DoDamage(dmg))
        {
            BuildingManager.Instance.DestroyBuilding(m_data);
        }
    }

    public Building Data
    {
        get
        {
            return m_data;
        }
        set
        {
            m_data = value;
        }
    }
}
