using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BtnManager : MonoBehaviour
{
    [Header("Options")]
    public GameObject optionsPanel;
    public GameObject settignsScroll;

    [Header("Book")]
    public GameObject bookPanel;
    public GameObject bookInfo;

    public GameObject materialsPanel;
    public TextMeshProUGUI materialsTemplate;

    public GameObject potionsScroll;
    public GameObject itemsScroll;

    [Header("Fullscreen")]
    public Toggle toggle;

    [Header("Resolutions")]
    public TMP_Dropdown resDropdown;
    Resolution[] resolutions;

    void Start()
    {
        Screen.fullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }

        else
        {
            toggle.isOn = false;
        }

        LookRes();
    }

    public void Options()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(!optionsPanel.activeSelf);
        }
    }

    public void Settings()
    {
        if (settignsScroll != null)
        {
            settignsScroll.SetActive(!settignsScroll.activeSelf);
            optionsPanel.SetActive(!optionsPanel.activeSelf);
        }
    }

    public void OnFullScreen(bool fullScreen)
    {
        Screen.fullScreen = fullScreen;
        PlayerPrefs.SetInt("FullScreen", fullScreen ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Full Screen " + fullScreen);
    }

    public void LookRes()
    {
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentRes = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (Screen.fullScreen 
                && resolutions[i].width == Screen.currentResolution.width 
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
            }
        }

        resDropdown.AddOptions(options);
        resDropdown.value = currentRes;
        resDropdown.RefreshShownValue();

        resDropdown.value = PlayerPrefs.GetInt("numResolution", 0);
    }

    public void ChangeRes(int resIndex)
    {
        PlayerPrefs.SetInt("numResolution", resDropdown.value);
        
        Resolution res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
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

                    if (materialsPanel.activeSelf)
                    {
                        materialsTemplate.text = "";

                        for (int i = 0; i < btnInfo.itemData.recipe.requiredMaterials.Count; i++)
                        {
                            materialsTemplate.text += btnInfo.itemData.recipe.requiredMaterials[i].itemName;

                            if (i < btnInfo.itemData.recipe.requiredMaterials.Count - 1)
                            {
                                materialsTemplate.text += ", ";
                            }
                        }
                    }
                }
            }
        }
    }

    public void PotionsBookBtn()
    {
        potionsScroll.SetActive(true);
        materialsPanel.SetActive(true);
        itemsScroll.SetActive(false);
    }

    public void ItemsBookBtn()
    {
        potionsScroll.SetActive(false);
        materialsPanel.SetActive(false);
        itemsScroll.SetActive(true);
    }

    public void Exit()
    {
        Debug.Log("You closed the game");
        Application.Quit();
    }
}
