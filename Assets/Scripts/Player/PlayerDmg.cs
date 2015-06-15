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

    public bool isPlayer;

	private Directions _direction;

    public void Attack(int dir)
    {
        Debug.LogError("Attack: " + dir);
        switch (dir)
        {
            case 1:
            _leftDetect.GetComponent<HitDetection>().Attack();
            break;
            case 2:
            _rightDetect.GetComponent<HitDetection>().Attack();
            break;
            case 3:
            _downDetect.GetComponent<HitDetection>().Attack();
            break;
            case 4:
            _upDetect.GetComponent<HitDetection>().Attack();
            break;

        }
    }

    public void Kill()
    {
        CanvasManager.Instance.PopUp("COME ON!", "ARE YOU?: " + isPlayer);
        if (isPlayer)
        {
            CanvasManager.Instance.PopUp("DEAD!", "You have been killed, you will respawn in 10 seconds.");
            Network.Destroy(this.transform.parent.gameObject);
            SceneManager.Instance.Respawn();
        }
    }
}
