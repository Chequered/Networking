using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitDetection : MonoBehaviour 
{
	List<GameObject> LPlayers = new List<GameObject> ();


	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.tag == "Building" || col.tag == "Player") 
		{
			LPlayers.Add(col.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.tag == "Building" || col.tag == "Player") 
		{
			LPlayers.Remove (col.gameObject);
		}
	}

	public void Attack()
	{
		foreach (GameObject col in LPlayers) 
		{
			Destroy(col.gameObject);
		}

	}
}
