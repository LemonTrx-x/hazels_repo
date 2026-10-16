using UnityEngine;

public class InstantiateObj : MonoBehaviour
{
    public Transform spawn;
    public ItemData item;
    public int price;
    public int itemForce = 250;

    public void OnInstantiateObj()
    {
        if (item != null && item.shopStock > 0 && GameManager.Instance.coins >= price)
        {
            GameObject newItem;

            newItem = Instantiate(item.prefab, spawn.position, spawn.rotation);

            newItem.GetComponent<Rigidbody>().AddForce(spawn.forward * itemForce);

            GameManager.Instance.coins -= price;
            item.shopStock--;
        }
    }
}
