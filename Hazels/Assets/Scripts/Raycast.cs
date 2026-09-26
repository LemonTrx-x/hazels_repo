using UnityEngine;
using UnityEngine.InputSystem;

public class Raycast : MonoBehaviour
{
    [Header("Raycast hit")]
    public float rayDistance = 5f;
    public LayerMask layerMask;
    
    private Color rayColor = Color.red;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * rayDistance, rayColor);
    }

    public void RayCast(InputAction.CallbackContext callbackContext)
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(origin, direction, out hit, rayDistance, layerMask))
        {
            if (callbackContext.performed && hit.collider.CompareTag("Ground"))
            {
                Debug.Log("Looking at Ground");
            }
        }
    }
}
