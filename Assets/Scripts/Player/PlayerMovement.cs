using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour 
{
	private Rigidbody2D _playerRigid;
    private NetworkView _networkView;

	private float _movementspeed = 10;
    private Vector2 vel;
    private BuildMode _buildMode;
	public bool networkBool = true; 

	void Start()
	{
        _playerRigid = GetComponent<Rigidbody2D>();
        _networkView = GetComponent<NetworkView>();
        if (!_networkView.isMine)
            _playerRigid.isKinematic = true;
	}

    void Update()
    {
        if (_networkView.isMine)
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

        if(vel != Vector2.zero)
		    _playerRigid.MovePosition (_playerRigid.position + vel * Time.deltaTime );

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
        Vector3 syncVelocity = Vector3.zero;
        if (stream.isWriting)
        {
            syncPosition = _playerRigid.position;
            stream.Serialize(ref syncPosition);

            syncVelocity = _playerRigid.velocity;
            stream.Serialize(ref syncVelocity);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncVelocity);

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncStartPosition = syncPosition + syncVelocity * syncDelay;
            syncEndPosition = syncPosition;
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
