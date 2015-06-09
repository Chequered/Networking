using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour, BuildingInterface 
{
	private int _health = 60;
	private float _shootDelay = 2f;
	private float _distance;

	public Transform prefab;
	
	void Update () 
	{

		TakeDmg ();
	
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

	public void TakeDmg()
	{
		//Update UI here
		//Play Turret Animations
	}

	public void OnDeath()
	{
		Destroy (this.gameObject);
	}

	void Shoot()
	{
		//Debug.Log ("TurretShoot");
		Debug.Log (Resources.Load ("Projectiles/TurretProjectile", typeof(GameObject)));
		GameObject.Instantiate (Resources.Load("Projectiles/TurretProjectile", typeof(GameObject)) as GameObject/*, this.gameObject.transform.position, this.gameObject.transform.rotation*/);
		Debug.Log ("TurretShoot");
	}
}
