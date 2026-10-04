using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Transform slotsContainer;

    [Header("Extra")]
    public Transform hotBarSoltContainer; //Only for the player
    public ItemData itemDev;
    public ItemData itemDev2;

    private List<Slot> slots = new List<Slot>();

    void Start()
    {
        slots.AddRange(slotsContainer.GetComponentsInChildren<Slot>());

        if (hotBarSoltContainer != null)
        {
            slots.AddRange(hotBarSoltContainer.GetComponentsInChildren<Slot>());
        }

        Debug.Log("Inventory initialized with " + slots.Count + " slots.");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            AddItem(itemDev, 1);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            AddItem(itemDev2, 1);
        }
    }

    public int AddItem(ItemData itemData, int stock)
    {
        int stockToSave = stock;
        
        //1. Search the slots with the same item and not full (to complete the stack)
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].itemData == itemData && slots[i].stock < itemData.maxStock)
            {
                //If we have 20 items, max stock is 50 and we want to add 10, we get 30 available spaces
                int availableSpace = itemData.maxStock - slots[i].stock;

                //Available space = 20 and we want to add 50 items, stock to store is 30
                int stockToStore = Mathf.Min(availableSpace, stockToSave);

                slots[i].SetItem(itemData, slots[i].stock + stock);
                stockToSave -= stockToStore;

                if (stockToSave <= 0)
                {
                    return 0;
                }
            }
        }

        //2. Search empty slots
        if (stockToSave > 0)
        {
            for (int i = 0;i < slots.Count; i++)
            {
                if (slots[i].itemData == null)
                {
                    int stockToStore = Mathf.Min(itemData.maxStock, stockToSave);

                    slots[i].SetItem(itemData, stockToStore);
                    stockToSave -= stockToStore;

                    if (stockToSave <= 0)
                    {
                        return 0;
                    }
                }
            }
        }

        //3. We have things left
        return stockToSave;
    }
}
