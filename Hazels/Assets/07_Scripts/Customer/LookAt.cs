using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform obj;

    void Start()
    {
        if (obj == null)
        {
            GameObject objFound = GameObject.Find("Player");
            obj = objFound.transform;
        }
    }

    void Update()
    {
        transform.GetComponent<Transform>().transform.LookAt(obj);
    }
}