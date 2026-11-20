using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class HandleCursors : MonoBehaviour
{
    public PlayerInput input;
    public GameObject virtualMouseUI;
    public GameManager gameManager;
    
    void Start()
    {
        
    }

    void Update()
    {
        OnHandleCursors();
    }

    public void OnHandleCursors()
    {
        if (gameManager != null)
        {
            if (gameManager.isGamePaused || gameManager.inMenu)
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

            else if (gameManager != null && gameManager.inMinigame)
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

        else
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
    }

    public void ResetVirtualMouse()
    {
        virtualMouseUI.GetComponent<VirtualMouseInput>().cursorTransform.position = new Vector2(Screen.width / 2, Screen.height / 2);
    }
}
