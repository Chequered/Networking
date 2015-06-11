using UnityEngine;
using System.Collections;

public class BuildingInfo : MonoBehaviour {

    private int m_gridX;
    private int m_gridY;
    private int m_typeID;


    [RPC]
    public void SetInfo(int x, int y, int buildingTypeID)
    {
        m_gridX = x;
        m_gridY = y;
        m_typeID = buildingTypeID;
    }

    [RPC]
    private void SyncBuilding(int x, int y, int buildingID, int teamID)
    {
        Building building = new Building(x, y);
        building.Build(TeamData.TeamColorByID(teamID), Building.TypeById(buildingID), this.gameObject);

        BuildingManager.Instance.AddBuilding(building);
        GridManager.Instance.RegisterBuilding(building.X, building.Y, building.Size, TeamData.TeamIDByColor(building.Team));

        if(teamID != NetworkManager.Instance.clientTeamID)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }
}
