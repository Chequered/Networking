using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb : MonoBehaviour 
{

	private List<GameObject> colsInAOE;

	void Start()
	{
		colsInAOE = new List<GameObject> ();
	}

	public void Explode()
	{
		foreach (GameObject col in colsInAOE) 
		{
			//dmg / Destory here
			if(col.tag ==  "Player")
			{
				col.transform.FindChild("Attack Zones").GetComponent<PlayerDmg>().Kill();
			}

			if(col.tag == "Building")
			{
				col.GetComponent<BuildingInfo>().Attack(30);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Player" || col.tag == "Building") 
		{
			colsInAOE.Add(col.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Player" || col.tag == "Building") 
		{
			colsInAOE.Remove(col.gameObject);
		}

	}
}
