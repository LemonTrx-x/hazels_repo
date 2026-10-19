using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnManager : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject bookPanel;

    public GameObject bookInfo;

    public void Play()
    {
        SceneManager.LoadScene("SCN_Gameplay");
    }

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

    public void Exit()
    {
        Debug.Log("You closed the game");
        Application.Quit();
    }
}
