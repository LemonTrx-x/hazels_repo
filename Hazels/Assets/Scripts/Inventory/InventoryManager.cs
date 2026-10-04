using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public GameObject playerInventoryUI;
    public Inventory playerInventory;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        playerInventory = GetComponent<Inventory>();
    }

    void Update()
    {
        
    }

    public void HandleInventoryUI(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            playerInventoryUI.SetActive(!playerInventoryUI.activeSelf);

            if (playerInventoryUI.activeSelf)
            {
                Cursor.lockState = CursorLockMode.None;
                GameManager.Instance.inMenu = true;
            }

            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                GameManager.Instance.inMenu = false;
            }
        }
    }
}
