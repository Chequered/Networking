using UnityEngine;
using System.Collections;

public class Grid {

    private int m_x;
    private int m_y;
    private Team m_ownedBy;
 
    public Grid(int x, int y, Team owner)
    {
        m_x = x;
        m_y = y;
        m_ownedBy = owner;
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

    public Team OwnedBy
    {
        get
        {
            return m_ownedBy;
        }
        set
        {
            m_ownedBy = value;
        }
    }
}
