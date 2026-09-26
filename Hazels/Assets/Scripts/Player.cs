using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 inputGamepad;

    [Header("Camera")]
    public Transform cameraPlayer;
    public GameObject mainCamera;

    public float gamepadSensitivity = 200f;
    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        inputGamepad = playerInput.actions["Look"].ReadValue<Vector2>() * gamepadSensitivity * Time.deltaTime;

        float mouseX = Mouse.current.delta.x.ReadValue() * mouseSensitivity * Time.deltaTime;
        float mouseY = Mouse.current.delta.y.ReadValue() * mouseSensitivity * Time.deltaTime;

        float inputX = mouseX + inputGamepad.x;
        float inputY = mouseY + inputGamepad.y;

        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraPlayer.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * inputX);
    }
}
