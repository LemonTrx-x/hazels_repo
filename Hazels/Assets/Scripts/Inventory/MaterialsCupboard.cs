
public class MaterialsCupboard : InteractiveObj
{
    public InventoryManager inventoryManager;
    
    public override void Interact()
    {
        inventoryManager.HandleInventoryUI();
    }
}
