using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public GameObject inventoryUI;
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

    public void HandleInventoryUI()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);

        if (inventoryUI.activeSelf)
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
