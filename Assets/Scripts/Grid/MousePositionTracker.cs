using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class MousePositionTracker : MonoBehaviour {


    private Vector3 m_mousePosition;
    private Ray m_ray;
    private GameObject m_gridSelector;
    private GameObject m_playerGridPoint;
    
    private void Start()
    {
        m_gridSelector = GameObject.Instantiate(Resources.Load("Grid/Grid Selection Block") as GameObject);
        m_playerGridPoint = transform.FindChild("GridPoint").gameObject;
    }

    private void Update()
    {
        if(m_playerGridPoint !=  null)
        {
            RaycastHit2D hit = Physics2D.Raycast(m_playerGridPoint.transform.position, transform.forward, 9999, LayerMask.GetMask("GridBase"));

            if (hit.transform != null)
            {
                if (hit.transform.tag == "Grid Base")
                {
                    m_gridSelector.SetActive(true);
                    m_mousePosition = ((Vector2)hit.transform.position) - hit.point;
                    int x = (int)Mathf.Floor((m_mousePosition.x + 50));
                    int y = (int)Mathf.Floor((m_mousePosition.y + 50));
                    m_gridSelector.transform.position = new Vector2(-(x - 49.5f), -(y - 49.5f));

                    if(Input.GetKeyUp(KeyCode.Space))
                    {
                        BuildingManager.Instance.BuildBuilding(x, y, Building.TypeById(1));
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
}
