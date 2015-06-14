using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitDetection : MonoBehaviour 
{
    private List<GameObject> entitiesInArea;
    public PlayerInfo m_player;

<<<<<<< HEAD
    private void Start()
    {
        entitiesInArea = new List<GameObject>();
    }
=======
	void Update()
	{
	}
>>>>>>> 53ff0d21d8ada1c096d45f3b5ddb70923c9a0f18

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Building") 
		{
            if (TeamData.TeamIDByColor(col.GetComponent<BuildingInfo>().Data.Team) != m_player.TeamID)
            {
                entitiesInArea.Add(col.gameObject);
            }
		}else if(col.tag == "Player")
        {
            if (col.GetComponent<PlayerInfo>().TeamID != m_player.TeamID)
            {
                entitiesInArea.Add(col.gameObject);
            }
        }
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Building" || col.tag == "Player") 
		{
            if(entitiesInArea.Contains(col.gameObject))
            {
                entitiesInArea.Remove(col.gameObject);
            }
		}
	}

	public void Attack()
    {
		foreach (GameObject col in entitiesInArea) 
		{
			if(col.transform.tag == "Player")
            {
                col.transform.FindChild("Attack Zones").GetComponent<PlayerDmg>().Kill();
            }else if(col.transform.tag == "Building")
            {
                col.GetComponent<BuildingInfo>().Attack(m_player.Damage);
            }
		}
	}
}
