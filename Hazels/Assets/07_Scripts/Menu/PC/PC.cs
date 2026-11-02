using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PC : MonoBehaviour
{
    [Header("Template")]
    public Transform orderParent;
    public GameObject orderTemplate;
    [HideInInspector] public List<PCReturnItem> returnList = new List<PCReturnItem>();

    void Start()
    {
        if (orderTemplate != null)
        {
            orderTemplate.SetActive(false);
        }
    }

    void Update()
    {
        
    }

    public void OrderEntry(Button clickedBtn)
    {
        PCNewItem newItemScript = clickedBtn.GetComponent<PCNewItem>();

        if (returnList.Count == 0)
        {
            CreateEntry(newItemScript);
            return;
        }

        foreach (PCReturnItem returnItem in returnList)
        {
            if (returnItem.itemData == newItemScript.itemData)
            {
                newItemScript.amount++;
                returnItem.itemAmount.text = newItemScript.amount.ToString();
                return;
            }
        }

        CreateEntry(newItemScript);
    }

    public void CreateEntry(PCNewItem newItem)
    {
        GameObject orderEntry;
        orderEntry = Instantiate(orderTemplate, orderParent);
        orderEntry.SetActive(true);
        PCReturnItem returnItemScript = orderEntry.GetComponent<PCReturnItem>();

        returnItemScript.itemSprite.sprite = newItem.itemData.icon;
        returnItemScript.itemName.text = newItem.itemData.itemName;
        newItem.amount++;
        returnItemScript.itemAmount.text = newItem.amount.ToString();
        returnItemScript.itemData = newItem.itemData;

        returnList.Add(returnItemScript);
    }
}
