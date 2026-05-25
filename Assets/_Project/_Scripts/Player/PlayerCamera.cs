using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{

    [SerializeField] private Transform playerCharacter;
    [SerializeField] private float sensitivity = 180f;
    private float currentXRotation = 0;

    void Start()
    {
        MouseInitializer();
    }

    void Update()
    {
        MouseMovement();
    }

    public void MouseInitializer()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        currentXRotation -= mouseY;
        currentXRotation = Mathf.Clamp(currentXRotation, -90, 90);

        this.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);
        playerCharacter.Rotate(Vector3.up * mouseX);
    }
}
