using TMPro;
using UnityEngine;

public class PCNewItem : MonoBehaviour
{
    [Header("Item Info")]
    public ItemData itemData;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textPrice;
    public int amount;

    void Start()
    {
        if (itemData != null)
        {
            textName.text = itemData.itemName;
            textPrice.text = itemData.price.ToString();
        }
    }
}
