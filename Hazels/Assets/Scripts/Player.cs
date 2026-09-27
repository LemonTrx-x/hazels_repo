using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 inputCamera;

    [Header("Camera")]
    public Transform cameraPlayer;
    public GameObject mainCamera;

    [Header("Sensitivities")]
    public float gamepadSensitivity = 200f;
    public float mouseSensitivity = 2f;

    float xRotation = 0f;
    bool isGamepad;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        OnControlsChanged(playerInput);
    }

    void LateUpdate()
    {
        if (isGamepad)
        {
            inputCamera = playerInput.actions["Look"].ReadValue<Vector2>() * gamepadSensitivity * Time.deltaTime;
        }

        else
        {
            inputCamera = playerInput.actions["Look"].ReadValue<Vector2>() * mouseSensitivity;
        }

        xRotation -= inputCamera.y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraPlayer.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * inputCamera.x);
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme == "Gamepad")
        {
            isGamepad = true;
            Debug.Log("Gamepad in use");
        }

        if (input.currentControlScheme == "Mouse&Keyboard")
        {
            isGamepad = false;
            Debug.Log("Mouse&Keyboard in use");
        }
    }
}
