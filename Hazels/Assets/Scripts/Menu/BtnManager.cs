using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject bookPanel;

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
                    Cursor.lockState = CursorLockMode.None;
                    GameManager.Instance.inMenu = true;
                }

                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    GameManager.Instance.inMenu = false;
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
