using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {

    public static BuildingManager Instance;

    private List<Building> m_buildings;

    private void Start()
    {
        Instance = this;
        m_buildings = new List<Building>();
    }

    public void BuildStartingBuildings()
    {

    }

    public void BuildBuilding(int x, int y, BuildingType type)
    {
        if(GridManager.Instance.CanBuild(x, y, Building.SizeByType(type)))
        {
            Building building = new Building(x, y);
            building.Build(TeamData.TeamColorByID(NetworkManager.Instance.clientTeamID), type);

            m_buildings.Add(building);
            GridManager.Instance.RegisterBuilding(building.X, building.Y, building.Size, NetworkManager.Instance.clientTeamID);
            GetComponent<NetworkView>().RPC("SyncNewBuilding", RPCMode.OthersBuffered, x, y, Building.IdByType(type), NetworkManager.Instance.clientTeamID);

            Network.Instantiate(Resources.Load("Buildings/Building Placeholder"), GridToWorld(x, y), Quaternion.identity, 0);
        }
    }

    [RPC]
    private void SyncNewBuilding(int x, int y, int buildingID, int teamID)
    {
        Building building = new Building(x, y);
        building.Build(TeamData.TeamColorByID(teamID), Building.TypeById(buildingID));

        m_buildings.Add(building);
        GridManager.Instance.RegisterBuilding(building.X, building.Y, building.Size, TeamData.TeamIDByColor(building.Team));
    }

	public void RemoveBuilding(Building building)
    {
        m_buildings.Remove(building);
        //do across network
    }

    private Vector2 GridToWorld(int gridX, int gridY)
    {
        Vector2 result = new Vector2(0, 0);
        result.x = -(gridX - 49.5f);
        result.y = -(gridY - 49.5f);

        return result;
    }
}
