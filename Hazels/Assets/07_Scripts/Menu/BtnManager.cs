using UnityEngine;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{
    [Header("Options")]
    public GameObject optionsPanel;

    [Header("Book")]
    public GameObject bookPanel;
    public GameObject bookInfo;
    public GameObject potionsScroll;
    public GameObject itemsScroll;

    public void Options()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(!optionsPanel.activeSelf);
        }
    }

    public void Book()
    {
        if (bookPanel != null)
        {
            if (!(GameManager.Instance.inMinigame || GameManager.Instance.isGamePaused))
            {
                bookPanel.SetActive(!bookPanel.activeSelf);

                if (bookPanel.activeSelf)
                {
                    GameManager.Instance.inMenu = true;
                }

                else
                {
                    GameManager.Instance.inMenu = false;
                }
            }
        }
    }

    public void BookInfo(Button clickedBtn)
    {
        if (bookInfo != null)
        {
            if (!(GameManager.Instance.inMinigame || GameManager.Instance.isGamePaused))
            {
                bookInfo.SetActive(!bookInfo.activeSelf);

                if (bookInfo.activeSelf)
                {
                    EntryTemplate btnInfo = clickedBtn.GetComponent<EntryTemplate>();
                    BookInfo bookInfoScript = bookInfo.GetComponent<BookInfo>();

                    bookInfoScript.objName.text = btnInfo.itemData.itemName;
                    bookInfoScript.objSprite.sprite = btnInfo.itemData.icon;
                    bookInfoScript.objInfo.text = btnInfo.itemData.info;
                    bookInfoScript.objFrase.text = btnInfo.itemData.frase;
                }
            }
        }
    }

    public void PotionsBookBtn()
    {
        potionsScroll.SetActive(true);
        itemsScroll.SetActive(false);
    }

    public void ItemsBookBtn()
    {
        potionsScroll.SetActive(false);
        itemsScroll.SetActive(true);
    }

    public void Exit()
    {
        Debug.Log("You closed the game");
        Application.Quit();
    }
}
