using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    public GameObject optionsPanel;
    
    public void Play()
    {
        SceneManager.LoadScene("SCN_Gameplay");
    }

    public void Options()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    public void Exit()
    {
        Debug.Log("You closed the game");
        Application.Quit();
    }
}
