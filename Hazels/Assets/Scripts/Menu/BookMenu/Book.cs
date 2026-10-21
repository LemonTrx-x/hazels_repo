using UnityEngine;

public class Book : MonoBehaviour
{
    [Header("Potions")]
    public Transform potionsEntryParent;
    public GameObject potionsEntryTemplate;

    [Header("Items")]
    public Transform itemsEntryParent;
    public GameObject itemsEntryTemplate;

    [Header("DataBases")]
    public ItemDataBase potionsDataBase;
    public ItemDataBase itemsDataBase;

    void Awake()
    {
        potionsEntryTemplate.gameObject.SetActive(false);
        itemsEntryTemplate.gameObject.SetActive(false);

        PotionBook();
        ItemBook();
    }

    public void PotionBook()
    {
        for (int i = 0; i < potionsDataBase.items.Count; ++i)
        {
            GameObject newPotionEntry;

            newPotionEntry = Instantiate(potionsEntryTemplate, potionsEntryParent);
            newPotionEntry.SetActive(true);

            EntryTemplate entryScript = newPotionEntry.GetComponent<EntryTemplate>();

            entryScript.objId.text = potionsDataBase.items[i].id.ToString();
            entryScript.objName.text = potionsDataBase.items[i].itemName;
            entryScript.objSprite.sprite = potionsDataBase.items[i].icon;
            entryScript.itemData = potionsDataBase.items[i];
        }
    }

    public void ItemBook()
    {
        for (int i = 0; i < itemsDataBase.items.Count; ++i)
        {
            GameObject newItemEntry;

            newItemEntry = Instantiate(itemsEntryTemplate, itemsEntryParent);
            newItemEntry.SetActive(true);

            EntryTemplate entryScript = newItemEntry.GetComponent<EntryTemplate>();

            entryScript.objId.text = itemsDataBase.items[i].id.ToString();
            entryScript.objName.text = itemsDataBase.items[i].itemName;
            entryScript.objSprite.sprite = itemsDataBase.items[i].icon;
            entryScript.itemData = itemsDataBase.items[i];
        }
    }
}
