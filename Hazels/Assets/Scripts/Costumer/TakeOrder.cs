
public class TakeOrder : InteractiveObj
{
    private Costumer costumer;
    
    public override void Interact()
    {
        costumer = GetComponent<Costumer>();
        costumer.GenerateOrder();
    }
}
