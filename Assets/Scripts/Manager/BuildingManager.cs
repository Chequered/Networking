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
        if (Network.isServer)
        {
            BuildBuilding(40, 40, Building.IdByType(BuildingType.Drill), TeamData.TeamIDByColor(Team.None));
        }
    }

    [RPC]
    public void BuildBuilding(int x, int y, int buildingID, int teamID)
    {
        if(Network.isServer)
        {
            Debug.Log(ServerMaster.Instance.Lobby.GetTeam(TeamData.TeamColorByID(teamID)));
            if(TeamData.TeamColorByID(teamID) != Team.None)
            {
                if (ServerMaster.Instance.Lobby.GetTeam(TeamData.TeamColorByID(teamID)).Resources < Building.CostById(buildingID))
                {
                    return;
                }
                else
                {
                    ServerMaster.Instance.Lobby.GetTeam(TeamData.TeamColorByID(teamID)).TakeResources(Building.CostById(buildingID));
                }
            }
            if(GridManager.Instance.CanBuild(x, y, Building.SizeByType(Building.TypeById(buildingID))))
            {
                string ResourcePath = "Buildings/" + Building.TypeById(buildingID).ToString() + "/" + TeamData.TeamColorByID(teamID).ToString();
                Debug.Log(ResourcePath);
                GameObject bObj = Network.Instantiate(Resources.Load(ResourcePath), GridToWorld(x, y), Quaternion.identity, 0) as GameObject;

                Building building = new Building(x, y);
                building.Build(TeamData.TeamColorByID(teamID), Building.TypeById(buildingID), bObj);

                m_buildings.Add(building);
                bObj.GetComponent<BuildingInfo>().SetInfo(x, y, buildingID, m_buildings.Count - 1, building);
                GridManager.Instance.RegisterBuilding(building.X, building.Y, building.Size, teamID);

                //Sync to other players
                bObj.GetComponent<NetworkView>().RPC("SyncBuilding", RPCMode.OthersBuffered, x, y, buildingID, teamID);
            }
        }
    }

    public void AddBuilding(Building building)
    {
        m_buildings.Add(building);
    }

    public void DestroyBuilding(Building building)
    {
        m_buildings.Remove(building);
        if(building.Type == BuildingType.HeadQuarters)
        {
            //TODO: Team loses.
        }
        Network.Destroy(building.GameObject);
    }

    public List<Building> GetBuildings()
    {
        return m_buildings;
    }

    private Vector2 GridToWorld(int gridX, int gridY)
    {
        Vector2 result = new Vector2(0, 0);
        result.x = -(gridX - 49.5f);
        result.y = -(gridY - 49.5f);

        return result;
    }
}
