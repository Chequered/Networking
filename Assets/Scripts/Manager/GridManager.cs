using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

	public const int WORLD_WIDTH = 100;
    public const int WORLD_HEIGHT = 100;

    private static Grid[,] m_grid;

    private void Start()
    {
        m_grid = new Grid[WORLD_WIDTH, WORLD_HEIGHT];
    }

    public static void GenerateGrid()
    {
        for (int x = 0; x < WORLD_WIDTH; x++)
        {
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                m_grid[x, y] = new Grid(x, y, Team.None);
            }
        }
    }

    public static void RegisterBuilding(Building building)
    {
        int startX = building.X;
        int startY = building.Y;
        int size = building.Size;

        if(size == 1)
        {
            m_grid[startX, startY].OwnedBy = building.Team;
        }
        else
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    m_grid[startX + x, startY + y].OwnedBy = building.Team;
                }
            }
        }
    }
}
