using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

    public static GridManager Instance;

	public const int WORLD_WIDTH = 100;
    public const int WORLD_HEIGHT = 100;

    private Grid[,] m_grid;
    private Grid m_hoveredGridBlock;

    private void Start()
    {
        m_grid = new Grid[WORLD_WIDTH, WORLD_HEIGHT];
        Instance = this;
    }

    public void GenerateGrid(bool buildAfterwards = true)
    {
        for (int x = 0; x < WORLD_WIDTH; x++)
        {
            for (int y = 0; y < WORLD_HEIGHT; y++)
            {
                m_grid[x, y] = new Grid(x, y, Team.None);
            }
        }
        if (buildAfterwards)
            Instantiate(Resources.Load("Grid/Grid Base") as GameObject, new Vector3(0, 0, 0), Quaternion.identity);
    }

    [RPC]
    public void RegisterBuilding(int bX, int bY, int bSize, int teamID)
    {
        int startX = bX;
        int startY = bY;
        int size = bSize;

        if(m_grid[bX, bY] == null)
        {
            GenerateGrid(false);
        }

        if(size == 1)
        {
            m_grid[startX, startY].OwnedBy = TeamData.TeamColorByID(teamID);
        }
        else
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    m_grid[startX + x, startY + y].OwnedBy = TeamData.TeamColorByID(teamID); ;
                }
            }
        }
    }

    public bool CanBuild(int bX, int bY, int bSize)
    {
        int startX = bX;
        int startY = bY;
        int size = bSize;

        bool avaiable = true;

        if(size == 1)
        {
            if (m_grid[bX, bY] != null)
            {
                if (m_grid[bX, bY].OwnedBy != Team.None)
                {
                    avaiable = false;
                }
            }
            else
            {
                avaiable = false;
            }
        }
        else
        {
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (m_grid[startX + x, startY + y] != null)
                    {
                        if (m_grid[startX + x, startY + y].OwnedBy != Team.None)
                        {
                            avaiable = false;
                        }
                    }
                    else
                    {
                        avaiable = false;
                    }
                }
            }
        }

        
        return avaiable;
    }

    public void SetHoveredGridBlock(int x, int y)
    {
        m_hoveredGridBlock = m_grid[x, y];
    }
}
