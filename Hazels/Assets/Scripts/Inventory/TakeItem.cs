using UnityEngine;

public class TakeItem : InteractiveObj
{
    public ItemData itemData;
    public int stock;

    public override void Interact()
    {
        int stockLeft = InventoryManager.Instance.playerInventory.AddItem(itemData, stock);

        if (stockLeft == 0)
        {
            Destroy(gameObject);
            return;
        }

        stock = stockLeft;
    }
}
