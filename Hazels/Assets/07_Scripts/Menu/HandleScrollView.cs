using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandleScrollView : MonoBehaviour
{
    public float gamePadScrollSens = 0.05f;
    public float mouseScrollSens = 10f;

    public PlayerInput input;
    

    void Update()
    {
        OnHandleScrollForce();
    }

    public void OnHandleScrollForce()
    {
        if (input.currentControlScheme == "Gamepad")
        {
            gameObject.GetComponentInChildren<ScrollRect>().scrollSensitivity = gamePadScrollSens;
        }

        else
        {
            gameObject.GetComponentInChildren<ScrollRect>().scrollSensitivity = mouseScrollSens;
        }
    }
}
