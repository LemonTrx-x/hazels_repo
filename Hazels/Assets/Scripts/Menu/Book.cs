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

            newPotionEntry.GetComponent<EntryTemplate>().objId.text = potionsDataBase.items[i].id.ToString();
            newPotionEntry.GetComponent<EntryTemplate>().objName.text = potionsDataBase.items[i].name;
            newPotionEntry.GetComponent<EntryTemplate>().objSprite.sprite = potionsDataBase.items[i].icon;
        }
    }
}
