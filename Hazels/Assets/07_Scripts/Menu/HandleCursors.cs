using UnityEngine;
using UnityEngine.InputSystem;

public class HandleCursors : MonoBehaviour
{
    public PlayerInput input;
    public GameObject virtualMouseUI;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (input.currentControlScheme == "Gamepad")
        {
            virtualMouseUI.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        else
        {
            virtualMouseUI.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
