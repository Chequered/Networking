using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class MousePositionTracker : MonoBehaviour {


    private Vector3 m_mousePosition;
    private Ray m_ray;
    private GameObject m_gridSelector;
    
    private void Start()
    {
        m_gridSelector = GameObject.Instantiate(Resources.Load("Grid/Grid Selection Block") as GameObject);
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 9999, LayerMask.GetMask("GridBase"));

        if (hit.transform != null)
        {
            if (hit.transform.tag == "Grid Base")
            {
                m_gridSelector.SetActive(true);
                m_mousePosition = ((Vector2)transform.position) - hit.point;
                int x = (int)Mathf.Floor((m_mousePosition.x + 50));
                int y = (int)Mathf.Floor((m_mousePosition.y + 50));
                m_gridSelector.transform.position = new Vector2(-(x - 49.5f), -(y - 49.5f));
                if(Input.GetMouseButtonUp(0))
                {
                    BuildingManager.Instance.BuildBuilding(x, y, new Player("Player Man", Team.Green), Building.TypeById(1));
                }
            }
            else
            {
                m_gridSelector.SetActive(false);
            }
        }
        else
        {
            m_gridSelector.SetActive(false);
        }
    }
}
