using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public Image bar;

    AsyncOperation operation;

    public void OnLoadScene(int scene)
    {
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
