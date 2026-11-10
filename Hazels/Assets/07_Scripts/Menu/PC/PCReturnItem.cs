using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PCReturnItem : MonoBehaviour
{
    [Header("Item Data")]
    public PCNewItem newItemScript;
    public ItemData itemData;
    public Image itemSprite;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemAmount;
    private PC pc;
    [HideInInspector] public int amount;

    void Start()
    {
        pc = GetComponentInParent<PC>();
    }

    public void ReturnEntry()
    {
        AudioManager.Instance.PlaySFX(1); //BtnClicked

        GameManager.Instance.coins += itemData.price * newItemScript.amount;
        newItemScript.amount = 0;
        pc.returnList.Remove(gameObject.GetComponent<PCReturnItem>());
        Destroy(gameObject);
    }
}
