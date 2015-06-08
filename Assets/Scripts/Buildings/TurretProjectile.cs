using UnityEngine;
using System.Collections;

public class TurretProjectile : MonoBehaviour {

	private float _lifeTime;
	private float _speed = 10f;
	
	void Update () 
	{
		_lifeTime -= 1 * Time.deltaTime;
		transform.Translate(Vector3.forward * _speed * Time.deltaTime);
		
		if (_lifeTime <= 0f) 
		{
			Destroy(this.gameObject);
		}
	
	}
}
