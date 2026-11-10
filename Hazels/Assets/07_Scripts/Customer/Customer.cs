using UnityEngine;
using System.Collections.Generic;

public class Customer : MonoBehaviour
{
    [Header("Costumer")]
    [HideInInspector] public List<ItemData> customerOrder = new List<ItemData>();

    public ItemDataBase potionsDataBase;
    [HideInInspector] public List<ItemData> potions = new List<ItemData>();
    public int maxPotions = 1;
    [HideInInspector] public string potionsNames = "";
    public int orderCoins = 100;
    public int orderScore = 100;

    public GameObject midPosition;
    public GameObject endPosition;
    public int speed = 2;
    public bool goToMid = false;
    public bool finished = false;
    public bool goodFinish = false;
    public bool badFinish = false;

    [Header("Costumer Lines")]
    public string[] customerLines;
    public string[] goodLines;
    public string[] badLines;

    [Header("Player")]
    [HideInInspector] public List<Slot> hotBarSlots = new List<Slot>();
    [HideInInspector] public List<ItemData> potionsToCheck = new List<ItemData>();
    
    void Start()
    {
        goToMid = true;

        for (int i = 0; i < potionsDataBase.items.Count; i++)
        {
            potions.Add(potionsDataBase.items[i]);
        }
    }

    void Update()
    {
        MoveToMid();
        MoveToEnd();
    }

    public void GenerateOrder()
    {
        customerOrder.Clear();

        if (potions == null || potions.Count == 0 || maxPotions <= 0)
        {
            Debug.LogWarning("No potions available or maxPotions = 0.");
            return;
        }

        int amountToOrder = Random.Range(1, Mathf.Min(maxPotions, potions.Count) + 1);

        for (int i = 0; i < amountToOrder; i++)
        {
            ItemData randomPotion = potions[Random.Range(0, potions.Count)];
            customerOrder.Add(randomPotion);
        }

        for (int i = 0; i < customerOrder.Count; i++)
        {
            potionsNames += customerOrder[i].itemName;

            if (i < customerOrder.Count - 1)
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

        foreach (ItemData item in customerOrder)
        {
            if (potionsToCheck.Contains(item))
            {
                potionsToCheck.Remove(item);
            }
        }

        if (potionsToCheck.Count == 0)
        {
            finished = true;
            goodFinish = true;
            GameManager.Instance.score += orderScore;
            GameManager.Instance.coins += orderCoins;

            AudioManager.Instance.PlaySFX(9); //Coins
        }

        else
        {
            finished = true;
            badFinish = true;
            GameManager.Instance.score += orderScore/4;
            GameManager.Instance.coins += orderCoins/4;

            AudioManager.Instance.PlaySFX(9); //Coins
        }
    }

    public void MoveToMid()
    {
        float distance = Vector3.Distance(this.transform.position, midPosition.transform.position);

        if (distance <= 0.01)
        {
            goToMid = false;
        }
        
        if (goToMid == true)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, midPosition.transform.position, speed * Time.deltaTime);
        }
    }
    
    public void MoveToEnd()
    {
        if (finished == true)
        {
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
