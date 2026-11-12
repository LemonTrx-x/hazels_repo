using UnityEngine;

public class OpenUI : InteractiveObj
{
    public GameObject newInventoryUI;
    public int sxfToPlay;

    public override void Interact()
    {
        AudioManager.Instance.PlaySFX(sxfToPlay);

        if (!GameManager.Instance.inMinigame)
        {
            InventoryManager.Instance.inventoryUI = newInventoryUI;
            InventoryManager.Instance.HandleInventoryUI();
        }
    }
}
