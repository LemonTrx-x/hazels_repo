using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Transform slotsContainer;

    [Header("Extra")]
    public Transform hotBarSoltContainer; //Only for the player
    public ItemData itemDev;

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
    }

    public int AddItem(ItemData itemData, int stock)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].itemData == null)
            {
                slots[i].SetItem(itemData, stock);
                return 0;
            }
        }

        return stock;
    }
}
