using UnityEngine;

public class InstantiateCostumer : MonoBehaviour
{
    public Transform spawn;
    public GameObject costumer = null;
    public GameObject costumerPrefab;
    public GameObject merchantPrefab;
    public float specialChance = 20f;
    float chance;

    public int costumersLeft = 1;

    public GameObject hotBarPlayer;
    public GameObject midPos;
    public GameObject endPos;

    void Update()
    {
        if (costumer == null && costumersLeft > 0)
        {
            chance = Random.value;
            GameObject prefabToUse = chance <= (specialChance / 100) ? merchantPrefab : costumerPrefab;
            
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
            newCostumer.GetComponent<Costumer>().hotBarSlots.AddRange(hotBarPlayer.GetComponentsInChildren<Slot>());
            newCostumer.GetComponent<Costumer>().midPosition = midPos;
            newCostumer.GetComponent<Costumer>().endPosition = endPos;
        }

        costumer = newCostumer;
        costumersLeft--;
    }
}
