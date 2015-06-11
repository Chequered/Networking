using UnityEngine;
using System.Collections;

public enum Directions
{
	up,
	left,
	right,
	down
}

public class PlayerDmg : MonoBehaviour 
{
	[SerializeField] private GameObject _upDetect;
	[SerializeField] private GameObject _leftDetect;
	[SerializeField] private GameObject _rightDetect;
	[SerializeField] private GameObject _downDetect;

	private Directions _direction;

	void Update()
	{
		switch (_direction) 
		{
			//case Directions.up:_upDetect	//getComponent<HitDetection>().hitDetected();
			//break;
			//case Directions.left:_leftDetect //getComponent<HitDetection().HitDetected();
			//break;
			//case Directions.right:_rightDetect //getComponent<HitDetection().hitDetected();
			//break;
			//case Directions.down: _DownDetect //getComponent<HitDetection().hitDetected();
		}
	}


	void OnTriggerEnter(Collider col)
	{
		if (Input.GetKeyDown (KeyCode.UpArrow)) 
		{
			_upDetect.GetComponent<HitDetection>().Attack();
		}

		if (Input.GetKeyDown (KeyCode.LeftArrow)) 
		{
			_leftDetect.GetComponent<HitDetection>().Attack();
		}

		if (Input.GetKeyDown (KeyCode.RightArrow)) 
		{
			_rightDetect.GetComponent<HitDetection>().Attack();
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) 
		{
			_downDetect.GetComponent<HitDetection>().Attack();
		}
	}
}
