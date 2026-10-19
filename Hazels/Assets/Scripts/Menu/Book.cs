using UnityEngine;

public class Book : MonoBehaviour
{
    public Transform potionsEntryParent;
    public GameObject potionsEntryTemplate;

    public ItemDataBase potionsDataBase;

    void Awake()
    {
        potionsEntryTemplate.gameObject.SetActive(false);

        StartBook();
    }

    public void StartBook()
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
}
