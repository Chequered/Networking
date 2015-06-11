using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	private Rigidbody2D _playerRigid;
    private NetworkView _networkView;

<<<<<<< HEAD
	private float _movementspeed = 10;
    private Vector2 vel;
    private BuildMode _buildMode;
=======
	public bool networkBool = true; 

	private float _movementspeed = 5;
	Vector2 vel;
>>>>>>> ed1e278210529bc52ead83b940263624d2df540d

	void Start()
	{
        _playerRigid = GetComponent<Rigidbody2D>();
        _networkView = GetComponent<NetworkView>();
	}

	void Update()
	{
        if(_networkView.isMine || networkBool == false)
        {
            PlayerMoveInput();
        }
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

    private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        if (stream.isWriting)
        {
            syncPosition = _playerRigid.position;
            stream.Serialize(ref syncPosition);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            _playerRigid.position = syncPosition;
        }
    }

    private void BuildBuilding()
    {
        if(BuildingManager.Instance.BuildBuilding(0, 0, Building.TypeByMode(_buildMode)) != null)
        {
            //play anim;
        }
    }
}
