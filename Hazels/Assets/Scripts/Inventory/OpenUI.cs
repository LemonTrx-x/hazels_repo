
using UnityEngine;

public class OpenUI : InteractiveObj
{
    public GameObject newInventoryUI;

    public override void Interact()
    {
        InventoryManager.Instance.inventoryUI = newInventoryUI;
        InventoryManager.Instance.HandleInventoryUI();
    }
}
