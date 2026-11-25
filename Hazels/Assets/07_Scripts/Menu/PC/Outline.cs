using UnityEngine;

public class Outline : MonoBehaviour
{
    public GameObject outline;

    void Awake()
    {
        if (outline != null) outline.SetActive(false);
    }
}
