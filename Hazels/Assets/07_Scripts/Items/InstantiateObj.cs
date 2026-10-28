using UnityEngine;
using TMPro;

public class InstantiateObj : MonoBehaviour
{
    public Transform spawn;
    public ItemData item;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI shopStockText;

    public int itemForce = 250;

    void Start()
    {
        if (item != null) { nameText.text = item.itemName; }
    }

    void Update()
    {
        if (item != null) { shopStockText.text = item.shopStock.ToString(); }
    }

    public void OnInstantiateObj()
    {
        if (item != null && item.shopStock > 0)
        {
            GameObject newItem;

            newItem = Instantiate(item.prefab, spawn.position, spawn.rotation);

            newItem.GetComponent<Rigidbody>().AddForce(spawn.forward * itemForce);

            item.shopStock--;
        }
    }
}
