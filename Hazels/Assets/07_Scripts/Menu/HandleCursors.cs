using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class HandleCursors : MonoBehaviour
{
    public PlayerInput input;
    public GameObject virtualMouseUI;
    
    void Start()
    {
        
    }

    void Update()
    {
        OnHandleCursors();
    }

    public void OnHandleCursors()
    {
        if (GameManager.Instance.isGamePaused || GameManager.Instance.inMenu)
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
                ResetVirtualMouse();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        else if (GameManager.Instance.inMinigame)
        {
            if (input.currentControlScheme == "Gamepad")
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }

            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        else
        {
            virtualMouseUI.SetActive(false);
            ResetVirtualMouse();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ResetVirtualMouse()
    {
        virtualMouseUI.GetComponent<VirtualMouseInput>().cursorTransform.position = new Vector2(Screen.width / 2, Screen.height / 2);
    }
}
