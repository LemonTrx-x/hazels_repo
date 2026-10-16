using UnityEngine;
using System.Collections.Generic;

public class Costumer : MonoBehaviour
{
    [Header("Costumer")]
    [HideInInspector] public List<ItemData> costumerOrder = new List<ItemData>();

    public ItemDataBase potionsDataBase;
    [HideInInspector] public List<ItemData> potions = new List<ItemData>();
    public int maxPotions = 1;
    [HideInInspector] public string potionsNames = "";

    public GameObject endPosition;
    public int speed = 2;
    public bool finished = false;

    [Header("Player")]
    [HideInInspector] public List<Slot> hotBarSlots = new List<Slot>();
    [HideInInspector] public List<ItemData> potionsToCheck = new List<ItemData>();
    
    void Start()
    {
        for (int i = 0; i < potionsDataBase.items.Count; i++)
        {
            potions.Add(potionsDataBase.items[i]);
        }
    }

    void Update()
    {
        MoveToEnd();
    }

    public void GenerateOrder()
    {
        costumerOrder.Clear();

        if (potions == null || potions.Count == 0 || maxPotions <= 0)
        {
            Debug.LogWarning("No potions available or maxPotions = 0.");
            return;
        }

        int amountToOrder = Random.Range(1, Mathf.Min(maxPotions, potions.Count) + 1);

        for (int i = 0; i < amountToOrder; i++)
        {
            ItemData randomPotion = potions[Random.Range(0, potions.Count)];
            costumerOrder.Add(randomPotion);
        }

        for (int i = 0; i < costumerOrder.Count; i++)
        {
            potionsNames += costumerOrder[i].name;

            if (i < costumerOrder.Count - 1)
            {
                potionsNames += ", ";
            }
        }

        Debug.Log("My order is: " + potionsNames);
    }

    public void CheckOrder()
    {
        for (int i = 0; i < hotBarSlots.Count; i++)
        {
            if (hotBarSlots[i].itemData != null)
            {
                potionsToCheck.Add(hotBarSlots[i].itemData);
                hotBarSlots[i].ClearItem();
            }
        }

        foreach (ItemData item in costumerOrder)
        {
            if (potionsToCheck.Contains(item))
            {
                potionsToCheck.Remove(item);
            }
        }

        if (potionsToCheck.Count == 0)
        {
            finished = true;
        }
    }

    public void MoveToEnd()
    {
        if (finished == true)
        {
            Debug.Log("Thanks for the potions :>!");
            this.transform.position = Vector3.Lerp(this.transform.position, endPosition.transform.position, speed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndPosition"))
        {
            finished = false;
            Destroy(gameObject);
        }
    }
}
