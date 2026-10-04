using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public ItemData itemData;
    [HideInInspector] public int stock;

    public Image icon;
    public TextMeshProUGUI stockText;

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (itemData == null)
        {
            return;
        }

        stockText.text = "";
        icon.enabled = false;
        UIManager.Instance.ghostIcon.enabled = true;
        UIManager.Instance.ghostIcon.sprite = itemData.icon;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (itemData == null)
        {
            return;
        }

        UIManager.Instance.ghostIcon.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (itemData == null)
        {
            return;
        }

        icon.enabled = true;
        UIManager.Instance.ghostIcon.enabled= false;
        stockText.text = stock.ToString();

        //If it didn't land on a slot, we send it back to origin
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Slot"))
        {
            Slot slotDestiny = eventData.pointerEnter.GetComponent<Slot>();

            if (slotDestiny != null && slotDestiny != this)
            {
                //If empty we save it here
                if (slotDestiny.itemData == null)
                {
                    slotDestiny.SetItem(itemData, stock);
                    CleatItem();
                    return;
                }

                //If is the same slot, see if we can add all or some
                else if (slotDestiny.itemData == itemData)
                {
                    if (slotDestiny.stock + stock <= itemData.maxStock) //We could add all
                    {
                        slotDestiny.SetItem(itemData, slotDestiny.stock + stock);
                        CleatItem();
                    }

                    else //If not, we add as much as possible
                    {
                        int stockToMove = itemData.maxStock - slotDestiny.stock;
                        slotDestiny.SetItem(itemData, itemData.maxStock);
                        this.SetItem(itemData, stock - stockToMove);
                    }
                }

                //If not empty nor the same item
                else
                {
                    ItemData temporaryItemData = slotDestiny.itemData;

                    int temporaryStock = slotDestiny.stock;

                    slotDestiny.SetItem(itemData, stock);
                    this.SetItem(temporaryItemData, temporaryStock);
                }
            }
        }
    }
}
