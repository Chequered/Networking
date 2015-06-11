using UnityEngine;
using System.Collections;

public class TurretProjectile : MonoBehaviour {

	private float _lifeTime = 10f;
	private float _speed = 0.4f;

	[SerializeField] private GameObject _playerTemp;

	void Update () 
	{
		_lifeTime -= 1 * Time.deltaTime;

		this.transform.Translate (new Vector2(0,1) * _speed * Time.deltaTime);

		//transform.Translate (transform.forward * _speed * Time.deltaTime);

		if (_lifeTime <= 0f) 
		{
			Destroy(this.gameObject);
		}
	
	}

	public void SetTarget(GameObject Target)
	{
		Vector3 dir = transform.position - Target.transform.position;
		float angle = (Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg) + 90;

		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


		//transform.LookAt (Target.transform.position);
		//transform.Translate (Vector2.MoveTowards (this.gameObject.transform.position, Target.transform.position, _speed * Time.deltaTime));
	}
}
