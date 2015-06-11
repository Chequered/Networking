using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Turret : MonoBehaviour, BuildingInterface 
{
	private int _health = 60;
	private float _shootDelay = 10f;
	private float _distance;

	void Update () 
	{

		TakeDmg ();
	}

	void OnTriggerEnter2D(Collider2D col)
	{

	}

	void OnTriggerStay2D(Collider2D col)
	{
		_shootDelay -= 1 * Time.deltaTime;
		
		if (col.tag == "Player" &&_shootDelay <= 0f || col.tag == "Building" && _shootDelay <= 0f) 
		{
			Shoot(col.gameObject);
			_shootDelay = 10f;
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

	void Shoot(GameObject Target)
	{
		GameObject bullet = GameObject.Instantiate (Resources.Load ("Projectiles/TurretProjectile") as GameObject, this.gameObject.transform.position, this.gameObject.transform.rotation) as GameObject;
		bullet.GetComponent<TurretProjectile> ().SetTarget (Target); 
		Debug.Log ("TurretShoot");
	}
}
