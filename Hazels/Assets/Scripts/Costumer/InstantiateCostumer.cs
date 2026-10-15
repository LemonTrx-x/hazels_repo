using UnityEngine;

public class InstantiateCostumer : MonoBehaviour
{
    public Transform spawn;
    public GameObject costumerObj;
    public int itemForce = 250;

    public int costumersLeft = 1;

    public GameObject hotBarPlayer;
    public GameObject endPos;

    void Start()
    {
        OnInstantiateCostumer();
    }

    public void OnInstantiateCostumer()
    {
        if (costumerObj != null && costumersLeft > 0)
        {
            GameObject newCostumer;

            newCostumer = Instantiate(costumerObj, spawn.position, spawn.rotation);
            newCostumer.GetComponent<Costumer>().hotBarSlots.AddRange(hotBarPlayer.GetComponentsInChildren<Slot>());
            newCostumer.GetComponent<Costumer>().endPosition = endPos;

            costumersLeft--;
        }
    }
}
