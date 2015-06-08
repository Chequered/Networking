using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{

	[SerializeField] private GameObject _player;
	private Rigidbody2D _playerRigid;

	private float _movementspeed = 2;
	Vector2 vel;

	void Start()
	{

		_playerRigid = _player.GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		PlayerMoveInput ();
	}

	void PlayerMoveInput()
	{
		vel = Vector2.zero;

		if(Input.GetKey(KeyCode.W))
		{
			vel.y +=  _movementspeed;
		}

		if(Input.GetKey(KeyCode.S))
		{
			vel.y -= _movementspeed;
		}

		if(Input.GetKey(KeyCode.A))
		{
			vel.x -= _movementspeed;
		}

		if(Input.GetKey(KeyCode.D))
		{
			vel.x += _movementspeed;
		}
		_playerRigid.MovePosition (_playerRigid.position + vel * Time.deltaTime );

	}
}
