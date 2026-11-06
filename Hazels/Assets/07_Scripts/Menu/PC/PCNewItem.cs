using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PCNewItem : MonoBehaviour
{
    [Header("Item Info")]
    public ItemData itemData;
    public Image imageSprite;
    public TextMeshProUGUI textPrice;
    public int amount;

    void Start()
    {
        if (itemData != null)
        {
            imageSprite.sprite = itemData.icon;
            textPrice.text = itemData.price.ToString() + " Coins";
        }
    }
}
