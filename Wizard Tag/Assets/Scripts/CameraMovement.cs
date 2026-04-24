using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float mouseSensitivity;
    public float verticalRotation;
    public float horizntalRotation;
    public Camera playerCamera;
    public Transform player;
    void Update()
    {
        HandleMouseLook();
    }


    public void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        horizntalRotation += mouseX;

        playerCamera.transform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
        player.eulerAngles = new Vector3(player.eulerAngles.x, horizntalRotation, player.eulerAngles.z);
    }

}
