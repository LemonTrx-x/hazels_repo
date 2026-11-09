using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InstantiateObj : MonoBehaviour
{
    public Transform spawn;
    public ItemData item;
    public Image nameText;
    public TextMeshProUGUI shopStockText;

    public int itemForce = 250;

    void Start()
    {
        if (item != null) nameText.sprite = item.icon;
    }

    void Update()
    {
        if (item != null) shopStockText.text = item.shopStock.ToString();
    }

    public void OnInstantiateObj()
    {
        if (item != null && item.shopStock > 0)
        {
            GameObject newItem;

            newItem = Instantiate(item.prefab, spawn.position, spawn.rotation);

            newItem.GetComponent<Rigidbody>().AddForce(spawn.forward * itemForce);

            item.shopStock--;
            ToDoList.Instance.materialsTaken = true;
        }
    }
}
