using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class PC : MonoBehaviour
{
    [Header("Template")]
    public Transform orderParent;
    public GameObject orderTemplate;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI scoreText;
    [HideInInspector] public List<PCReturnItem> returnList = new List<PCReturnItem>();

    void Start()
    {
        if (orderTemplate != null)
        {
            orderTemplate.SetActive(false);
        }

        if (coinsText != null)
        {
            coinsText.text = "COINS: " + GameManager.Instance.coins.ToString();
        }

        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + GameManager.Instance.score.ToString();
        }
    }

    void Update()
    {
        if (coinsText != null)
        {
            coinsText.text = "COINS: " + GameManager.Instance.coins.ToString();
        }

        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + GameManager.Instance.score.ToString();
        }
    }

    public void OrderEntry(Button clickedBtn)
    {
        PCNewItem newItemScript = clickedBtn.GetComponent<PCNewItem>();

        if (returnList.Count == 0)
        {
            CreateEntry(newItemScript);
            return;
        }

        foreach (PCReturnItem returnItem in returnList)
        {
            if (GameManager.Instance.coins <= 0)
            {
                Debug.Log("Not enough money!");
                return;
            }
            
            if (returnItem.itemData == newItemScript.itemData)
            {
                newItemScript.amount++;
                returnItem.itemAmount.text = newItemScript.amount.ToString();
                GameManager.Instance.coins -= newItemScript.itemData.price;
                return;
            }
        }

        if (newItemScript.itemData != null)
        {
            CreateEntry(newItemScript);
        }
    }

    public void CreateEntry(PCNewItem newItem)
    {
        GameObject orderEntry;
        orderEntry = Instantiate(orderTemplate, orderParent);
        orderEntry.SetActive(true);
        PCReturnItem returnItemScript = orderEntry.GetComponent<PCReturnItem>();

        returnItemScript.itemSprite.sprite = newItem.itemData.icon;
        returnItemScript.itemName.text = newItem.itemData.itemName;

        newItem.amount++;
        returnItemScript.itemAmount.text = newItem.amount.ToString();
        GameManager.Instance.coins -= newItem.itemData.price;

        returnItemScript.newItemScript = newItem;
        returnItemScript.itemData = newItem.itemData;

        returnList.Add(returnItemScript);
    }
}
