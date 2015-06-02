using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingManager : MonoBehaviour {

    private static BuildingManager Instance;

    private List<Building> m_buildings;

    private void Start()
    {
        Instance = this;
        m_buildings = new List<Building>();
    }

    public void BuildBuilding(int x, int y, Player player, BuildingType type)
    {
        Building building = new Building(x, y, 100);
        building.Build(player, type);

        m_buildings.Add(building);
        GridManager.RegisterBuilding(building);
        //do across network
    }

	public void RemoveBuilding(Building building)
    {
        m_buildings.Remove(building);
        //do across network
    }

    public void AttackBuilding(Building building, Player player)
    {
        building.DealDamage(player.Damage);
        //do across network
    }
}
