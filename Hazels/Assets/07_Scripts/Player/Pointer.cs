using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    public GameObject pointer;

    public Sprite originalSpr;
    public Sprite gamepadSpr;
    public Sprite keyboardSpr;

    private GameObject objOutline;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Obj"))
        {
            if (other.GetComponent<Outline>() != null)
            {
                objOutline = other.GetComponent<Outline>().outline;
            }

            if (GameManager.Instance.input.currentControlScheme == "Gamepad")
            {
                pointer.GetComponent<Image>().sprite = gamepadSpr;
            }

            else
            {
                pointer.GetComponent<Image>().sprite = keyboardSpr;
            }

            if (objOutline != null)
            {
                objOutline.SetActive(true);
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

            if (objOutline != null)
            {
                objOutline.SetActive(false);
            }
        }
    }
}
