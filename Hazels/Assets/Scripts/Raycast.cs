using UnityEngine;
using UnityEngine.InputSystem;

public class Raycast : MonoBehaviour
{
    public Player player;

    [Header("Raycast hit")]
    public float rayDistance = 2.5f;
    public LayerMask layerMask;
    public RaycastHit hit;

    private Color rayColor = Color.red;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * rayDistance, rayColor);
    }

    public void RayCast(InputAction.CallbackContext callbackContext)
    {
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out hit, rayDistance, layerMask))
        {
            if (callbackContext.performed && hit.collider.CompareTag("Obj"))
            {
                InteractiveObj interactiveObj = hit.collider.gameObject.GetComponent<InteractiveObj>();

                if (interactiveObj != null && !GameManager.Instance.inMenu)
                {
                    interactiveObj.Interact();
                }
            }
        }
    }
}
