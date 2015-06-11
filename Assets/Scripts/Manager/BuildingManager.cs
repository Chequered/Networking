using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BuildMode
{
    None,
    Wall,
    Turret
}

public class BuildingManager : MonoBehaviour {

    public static BuildingManager Instance;

    private List<Building> m_buildings;
    private BuildMode m_buildMode;

    private void Start()
    {
        Instance = this;
        m_buildings = new List<Building>();
    }

    public void BuildStartingBuildings()
    {

    }

    public Building BuildBuilding(int x, int y, BuildingType type)
    {
        Building building = null;
        if(GridManager.Instance.CanBuild(x, y, Building.SizeByType(type)))
        {
            GameObject bObj = Network.Instantiate(Resources.Load("Buildings/" + type.ToString() + "/" + TeamData.TeamColorByID(NetworkManager.Instance.clientTeamID).ToString()), GridToWorld(x, y), Quaternion.identity, 0) as GameObject;

            building = new Building(x, y);
            building.Build(TeamData.TeamColorByID(NetworkManager.Instance.clientTeamID), type, bObj);

            m_buildings.Add(building);
            GridManager.Instance.RegisterBuilding(building.X, building.Y, building.Size, NetworkManager.Instance.clientTeamID);

            //Sync to other players
            bObj.GetComponent<NetworkView>().RPC("SyncBuilding", RPCMode.OthersBuffered, x, y, Building.IdByType(type), NetworkManager.Instance.clientTeamID);
        }
        return building;
    }

    public void AddBuilding(Building building)
    {
        m_buildings.Add(building);
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
