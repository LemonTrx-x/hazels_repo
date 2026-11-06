using UnityEngine;

public class Outline : MonoBehaviour
{
    public GameObject outline;

    void Awake()
    {
        outline.SetActive(false);
    }
}
