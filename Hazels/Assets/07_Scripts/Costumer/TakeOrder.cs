using UnityEngine;

public class TakeOrder : InteractiveObj
{
    private Costumer costumer;
    
    public override void Interact()
    {
        costumer = GetComponent<Costumer>();

        if (costumer.costumerOrder.Count == 0)
        {
            costumer.GenerateOrder();
        }

        for (int i = 0; i < costumer.hotBarSlots.Count; i++)
        {
            if (costumer.hotBarSlots[i].itemData != null)
            {
                costumer.CheckOrder();
            }
        }
    }
}
