using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    public void Options()
    {
        Debug.Log("Options");
    }

    public void Exit()
    {
        Debug.Log("You closed the game");
        Application.Quit();
    }
}
