using UnityEngine;

public class InstantiateCustomer : MonoBehaviour
{
    public Transform spawn;
    public GameObject costumer = null;
    public GameObject customerPrefab;
    public GameObject merchantPrefab;
    public float specialChance = 20f;
    float chance;

    public int customersLeft = 1;

    public GameObject hotBarPlayer;
    public GameObject midPos;
    public GameObject endPos;

    void Update()
    {
        if (costumer == null && customersLeft > 0)
        {
            chance = Random.value;
            GameObject prefabToUse = chance <= (specialChance / 100) ? merchantPrefab : customerPrefab;
            
            OnInstantiateCostumer(prefabToUse);
        }
    }

    public void OnInstantiateCostumer(GameObject prefab)
    {
        GameObject newCostumer;

        newCostumer = Instantiate(prefab, spawn.position, spawn.rotation);

        if (chance <= (specialChance / 100))
        {
            newCostumer.GetComponent<Merchant>().midPosition = midPos;
            newCostumer.GetComponent<Merchant>().endPosition = endPos;
        }

        else
        {
            newCostumer.GetComponent<Customer>().hotBarSlots.AddRange(hotBarPlayer.GetComponentsInChildren<Slot>());
            newCostumer.GetComponent<Customer>().midPosition = midPos;
            newCostumer.GetComponent<Customer>().endPosition = endPos;
        }

        costumer = newCostumer;
        customersLeft--;
    }
}
