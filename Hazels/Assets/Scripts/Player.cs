using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Raycast raycast;
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

    [Header("Hands")]
    public Transform hand;
    public GameObject handObj;
    public float dropForce = 250f;
    public float objScale = 2f;

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

    public void PickUpObj()
    {
        raycast.hit.collider.GetComponent<Rigidbody>().useGravity = false;
        raycast.hit.collider.GetComponent<Rigidbody>().isKinematic = true;
        raycast.hit.collider.GetComponent<Collider>().isTrigger = true;

        raycast.hit.collider.transform.position = hand.position;
        raycast.hit.collider.transform.localScale /= objScale;
        raycast.hit.collider.gameObject.transform.SetParent(hand);

        handObj = raycast.hit.collider.gameObject;
    }

    public void DropObj(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && handObj != null)
        {
            handObj.GetComponent<Rigidbody>().useGravity = true;
            handObj.GetComponent<Rigidbody>().isKinematic = false;
            handObj.GetComponent<Rigidbody>().AddForce(hand.transform.forward * dropForce);
            handObj.GetComponent<Collider>().isTrigger = false;

            handObj.transform.localScale *= objScale;
            handObj.transform.SetParent(null);

            handObj = null;
        }
    }
}
