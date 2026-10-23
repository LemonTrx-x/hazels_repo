using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public GameObject loadingPanel;
    public Image bar;

    AsyncOperation operation;

    void Start()
    {
        loadingPanel.SetActive(false);
    }
    
    public void OnLoadScene(int scene)
    {
        loadingPanel.SetActive(true);
        StartCoroutine(Loading(scene));
    }

    IEnumerator Loading(int scene)
    {
        operation = SceneManager.LoadSceneAsync(scene);

        while (!operation.isDone)
        {
            bar.fillAmount = operation.progress;
            yield return null;
        }
    }
}
