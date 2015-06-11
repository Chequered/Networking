using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class BuildControls : MonoBehaviour {

    private Ray m_ray;
    private GameObject m_gridSelector;
    private GameObject m_playerGridPoint;
    private BuildMode m_buildMode;
        
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
                    Vector3 rayPos = ((Vector2)hit.transform.position) - hit.point;
                    int x = (int)Mathf.Floor((rayPos.x + 50));
                    int y = (int)Mathf.Floor((rayPos.y + 50));
                    m_gridSelector.transform.position = new Vector2(-(x - 49.5f), -(y - 49.5f));

                    if(Input.GetKeyUp(KeyCode.Space))
                    {
                        if(m_buildMode != BuildMode.None)
                            BuildBuilding(x, y);
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
        if(Input.GetKeyUp(KeyCode.E))
        {
            m_buildMode = BuildMode.Turret;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            m_buildMode = BuildMode.Wall;
        }
    }

    private void BuildBuilding(int x, int y)
    {
        if (BuildingManager.Instance.BuildBuilding(0, 0, Building.TypeByMode(_buildMode)) != null)
        {
            //play anim;
        }
    }
}
