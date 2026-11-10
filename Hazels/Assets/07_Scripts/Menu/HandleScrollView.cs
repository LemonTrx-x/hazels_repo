using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HandleScrollView : MonoBehaviour
{
    public float gamePadScrollSens = 0.05f;
    public float mouseScrollSens = 10f;
    

    void Update()
    {
        OnHandleScrollForce();
    }

    public void OnHandleScrollForce()
    {
        if (GameManager.Instance.input.currentControlScheme == "Gamepad")
        {
            gameObject.GetComponentInChildren<ScrollRect>().scrollSensitivity = gamePadScrollSens;
        }

        else
        {
            gameObject.GetComponentInChildren<ScrollRect>().scrollSensitivity = mouseScrollSens;
        }
    }
}
