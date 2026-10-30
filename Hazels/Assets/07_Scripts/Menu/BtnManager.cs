using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    List<Resolution> resolutions;

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
        //For each width x height, it only takes the entry with the highest refreshRate value
        Dictionary<Vector2Int, Resolution> best = new Dictionary<Vector2Int, Resolution>();

        foreach (Resolution r in Screen.resolutions)
        {
            Vector2Int size = new Vector2Int(r.width, r.height);

            if (!best.TryGetValue(size, out Resolution existing) || r.refreshRateRatio.value > existing.refreshRateRatio.value)
            {
                best[size] = r;
            }
        }

        resolutions = new List<Resolution>(best.Values);

        resDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentRes = 0;

        for (int i = 0; i < resolutions.Count; i++)
        {
            int hz = Mathf.RoundToInt((float)resolutions[i].refreshRateRatio.value);

            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + hz + " Hz";
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
        PlayerPrefs.Save();

        Resolution res = resolutions[resIndex];
        FullScreenMode mode = Screen.fullScreen ? FullScreenMode.FullScreenWindow: FullScreenMode.Windowed;

        //Also applies the refreshRate of the resolution
        Screen.SetResolution(res.width, res.height, mode, res.refreshRateRatio);
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
        bookInfo.SetActive(false);
        materialsPanel.SetActive(true);
        itemsScroll.SetActive(false);
    }

    public void ItemsBookBtn()
    {
        potionsScroll.SetActive(false);
        bookInfo.SetActive(false);
        materialsPanel.SetActive(false);
        itemsScroll.SetActive(true);
    }
    
    public void Exit()
    {
        Debug.Log("You closed the game");
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
