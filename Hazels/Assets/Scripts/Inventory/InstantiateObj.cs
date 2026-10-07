using UnityEngine;

public class InstantiateObj : MonoBehaviour
{
    public Transform spawn;
    public ItemData item;
    public int itemForce = 250;

    public void OnInstantiateObj()
    {
        if (item != null)
        {
            GameObject newItem;

            newItem = Instantiate(item.prefab, spawn.position, spawn.rotation);

            newItem.GetComponent<Rigidbody>().AddForce(spawn.forward * itemForce);
        }
    }
}
