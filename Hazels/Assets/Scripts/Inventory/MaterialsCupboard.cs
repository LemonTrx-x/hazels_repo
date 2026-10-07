
public class MaterialsCupboard : InteractiveObj
{

    public override void Interact()
    {
        InventoryManager.Instance.HandleCupBoardInventoryUI();
    }
}
