using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public GameObject inventoryUI;
    public Inventory playerInventory;
    public GameObject virtualMouse;

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
            GameManager.Instance.inMenu = true;
        }

        else
        {
            GameManager.Instance.inMenu = false;
        }
    }
}
