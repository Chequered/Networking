using UnityEngine;
using System.Collections;

public class CameraMover : MonoBehaviour {

    public float movementSpeed;
    public float zoomSpeed;

    private Vector3 movePos = new Vector3(0, 0, 0);
    private void Update()
    {
        movePos.x = -(Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed);
        movePos.y = -(Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed);
        Camera.main.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomSpeed;

        transform.Translate(movePos);
    }
}
