using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    [HideInInspector] public ItemData itemData;
    [HideInInspector] public int stock;

    public Image icon;
    private TextMeshProUGUI stockText;

    void Start()
    {
        stockText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetItem(ItemData itemData, int stock)
    {
        this.itemData = itemData;
        this.stock = stock;

        icon.sprite = itemData.icon;
        stockText.text = stock.ToString();
    }

    public void CleatItem()
    {
        itemData = null;
        stock = 0;
        icon.sprite = null;
        stockText.text= "";
    }
}
