using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    public GameObject pointer;

    public Sprite originalSpr;
    public Sprite gamepadSpr;
    public Sprite keyboardSpr;

    public GameObject outline;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obj"))
        {
            if (GameManager.Instance.input.currentControlScheme == "Gamepad")
            {
                pointer.GetComponent<Image>().sprite = gamepadSpr;
            }

            else
            {
                pointer.GetComponent<Image>().sprite = keyboardSpr;
            }

            if (outline != null)
            {
                outline.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obj"))
        {
            if (GameManager.Instance.input.currentControlScheme == "Gamepad")
            {
                pointer.GetComponent<Image>().sprite = gamepadSpr;
            }

            else
            {
                pointer.GetComponent<Image>().sprite = keyboardSpr;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Obj"))
        {
            pointer.GetComponent<Image>().sprite = originalSpr;
        }
    }
}
