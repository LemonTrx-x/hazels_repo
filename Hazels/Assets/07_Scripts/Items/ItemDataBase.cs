using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New DataBase", menuName = "Inventory/DataBase")]
public class ItemDataBase : ScriptableObject
{
    //Items list
    public List<ItemData> items = new List<ItemData>();

    //Items dictionary with their name (hidden)
    private Dictionary<string, ItemData> itemDictionary = new Dictionary<string, ItemData>();

    public void InitializeDataBase()
    {
        itemDictionary.Clear();

        foreach (ItemData item in items)
        {
            if (string.IsNullOrEmpty(item.id.ToString()))
            {
                continue;
            }

            if (itemDictionary.ContainsKey(item.id.ToString()))
            {
                itemDictionary.Add(item.id.ToString(), item);
            }
        }

        Debug.Log("DataBase initialized with: " + itemDictionary.Count + " items.");
    }

    public ItemData SearchItem(string id)
    {
        //Security system: if dictionary empty, it starts it up
        if (itemDictionary.Count == 0 && items.Count > 0)
        {
            InitializeDataBase();
        }

        if (itemDictionary.TryGetValue(id.ToString(), out ItemData itemData))
        {
            return itemData;
        }

        else
        {
            return null;
        }
    }
}
