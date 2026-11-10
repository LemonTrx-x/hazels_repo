using UnityEngine;

public class OpenUI : InteractiveObj
{
    public GameObject newInventoryUI;
    public int sxfToPlay;

    public override void Interact()
    {
        AudioManager.Instance.PlaySFX(sxfToPlay);

        InventoryManager.Instance.inventoryUI = newInventoryUI;
        InventoryManager.Instance.HandleInventoryUI();
    }
}
