using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	private Rigidbody2D _playerRigid;
    private NetworkView _networkView;

	private float _movementspeed = 10;
    private Vector2 vel;
    private BuildMode _buildMode;
    private PlayerGraphics _g;

	private bool hasBomb = false;

	public bool networkBool = true; 

	void Start()
	{
        _playerRigid = GetComponent<Rigidbody2D>();
        _networkView = GetComponent<NetworkView>();
        transform.FindChild("Attack Zones").GetComponent<PlayerDmg>().isPlayer = _networkView.isMine;
        _g = GetComponent<PlayerGraphics>();
        if (!_networkView.isMine)
            _playerRigid.isKinematic = true;
	}

    void Update()
    {
        if (_networkView.isMine || networkBool)
        {
            PlayerMoveInput();
        }
        else
        {
            SyncedMovement();
        }
    }

    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;
        _playerRigid.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    }

	void PlayerMoveInput()
	{
		vel = Vector2.zero;

        vel.x = Input.GetAxis("Horizontal") * _movementspeed;
        vel.y = Input.GetAxis("Vertical") * _movementspeed;

        if(_g.State != AnimationState.Attack)
        {
            if(vel.x > 0.2f || vel.x < -0.2f || vel.y > 0.2f || vel.y < -0.2f)
            {
                _g.Walk();
                _networkView.RPC("Walk", RPCMode.AllBuffered);
            }
            else
            {
                _g.Idle();
                _networkView.RPC("Idle", RPCMode.AllBuffered);
            }

        }

		if (Input.GetKey (KeyCode.K)) //pickup bomb
		{
			hasBomb = true;
			GameObject bomb = new GameObject("bomb");
			bomb.AddComponent<Bomb>();
			bomb.transform.parent = transform;
			bomb.transform.position = Vector2.zero;
		}

		if (Input.GetKey (KeyCode.Space) && hasBomb) 
		{

			transform.FindChild("bomb").GetComponent<Bomb>().Explode();
			Destroy(this.gameObject);
		}

        if(vel != Vector2.zero)
        {
            _playerRigid.MovePosition(_playerRigid.position + vel * Time.deltaTime);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow)) //attack left
        {
            _g.SwitchDirection(-1);
            _g.Attack(1);

            _networkView.RPC("SwitchDirection", RPCMode.AllBuffered, -1);
            _networkView.RPC("Attack", RPCMode.AllBuffered, 1);
        }

		if(Input.GetKeyUp(KeyCode.RightArrow)) //attack right
        {
            _g.SwitchDirection(1);
            _g.Attack(2);

            _networkView.RPC("SwitchDirection", RPCMode.AllBuffered, 1);
            _networkView.RPC("Attack", RPCMode.AllBuffered, 2);
        }

		if (Input.GetKeyUp(KeyCode.UpArrow))//attack up
        {
            _g.Attack(3);
            _networkView.RPC("Attack", RPCMode.AllBuffered, 3);
        }

		if (Input.GetKeyUp(KeyCode.DownArrow))//attack down
        {
            _g.Attack(4);
            _networkView.RPC("Attack", RPCMode.AllBuffered, 4);
        }

	}

    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    private void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if(_playerRigid == null)
        {
            _playerRigid = GetComponent<Rigidbody2D>();
        }

        Vector3 syncPosition = Vector3.zero;
        if (stream.isWriting)
        {
            syncPosition = _playerRigid.position;
            stream.Serialize(ref syncPosition);
        }
        else
        {
            stream.Serialize(ref syncPosition);

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncStartPosition = _playerRigid.position;
            syncEndPosition = syncPosition;
        }
    }

    public Vector2 Velocity
    {
        get
        {
            return vel;
        }
    }
}
