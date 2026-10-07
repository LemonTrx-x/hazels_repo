using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Image ghostIcon;
    public InventoryManager inventoryManager;

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
    }

    public void CloseUI()
    {
        inventoryManager.HandleInventoryUI();
    }
}
