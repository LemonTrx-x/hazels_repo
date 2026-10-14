using UnityEngine;

public class InstantiateCostumer : MonoBehaviour
{
    public Transform spawn;
    public GameObject costumerObj;
    public Costumer costumer;
    public int itemForce = 250;

    public void OnInstantiateCostumer()
    {
        if (costumerObj != null && costumer.costumersLeft > 0)
        {
            GameObject newItem;

            newItem = Instantiate(costumerObj, spawn.position, spawn.rotation);

            costumer.costumersLeft--;
        }
    }
}
