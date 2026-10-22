using UnityEngine;

public class InstantiateCostumer : MonoBehaviour
{
    public Transform spawn;
    public GameObject costumer = null;
    public GameObject costumerPrefab;
    public int itemForce = 250;

    public int costumersLeft = 1;

    public GameObject hotBarPlayer;
    public GameObject endPos;

    void Update()
    {
        if (costumer == null && costumersLeft > 0)
        {
            OnInstantiateCostumer();
        }
    }

    public void OnInstantiateCostumer()
    {
        if (costumerPrefab != null && costumersLeft > 0)
        {
            GameObject newCostumer;

            newCostumer = Instantiate(costumerPrefab, spawn.position, spawn.rotation);
            newCostumer.GetComponent<Costumer>().hotBarSlots.AddRange(hotBarPlayer.GetComponentsInChildren<Slot>());
            newCostumer.GetComponent<Costumer>().endPosition = endPos;

            costumer = newCostumer;

            costumersLeft--;
        }
    }
}
