using UnityEngine;
using System.Collections.Generic;

public class Costumer : MonoBehaviour
{
    [Header("Costumer")]
    private List<ItemData> costumerOrder = new List<ItemData>();

    public ItemDataBase potionsDataBase;
    [HideInInspector] public List<ItemData> potions = new List<ItemData>();
    public int maxPotions = 1;

    public GameObject endPosition;
    public int speed = 5;
    
    public int costumersLeft = 1;
    
    void Start()
    {
        for (int i = 0; i < potionsDataBase.items.Count; i++)
        {
            potions.Add(potionsDataBase.items[i]);
        }
    }

    void Update()
    {
        
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

        string potionsNames = "";

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

    }
}
