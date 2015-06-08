using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour, BuildingInterface 
{
	private int _health = 60;
	private float _shootDelay = 2f;
	private float _distance;

	void Start () 
	{
	
	}

	void Update () 
	{

		TakeDmg ();
		OnDeath ();
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		_shootDelay -= 1 * Time.deltaTime;

		if (col.tag == "Player" || col.tag == "Building") 
		{
			Shoot();
			_shootDelay = 2f;
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{

	}

	public void TakeDmg()
	{

	}

	public void OnDeath()
	{

	}

	void Shoot()
	{
		Debug.Log ("Kappa");
		//GameObject.Instantiate(TurretArrow,
	}
}
