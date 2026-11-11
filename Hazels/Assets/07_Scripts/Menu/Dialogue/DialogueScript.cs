using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueScript : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI skipText;
    public string[] lines;
    public float textSpeed = 0.25f;

    public int index;

    void Update()
    {
        ChangeSkipText();
    }

    public void NextDialogue(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && GameManager.Instance.inDialogue)
        {
            if (dialogueText.text == lines[index])
            {
                NextLine();
            }

            else
            {
                StopAllCoroutines();
                dialogueText.text = lines[index];
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        dialogueText.text = string.Empty;

        dialoguePanel.SetActive(true);
        GameManager.Instance.inDialogue = true;
        StartCoroutine(WriteLine());
    }

    IEnumerator WriteLine()
    {
        foreach (var letter in lines[index].ToCharArray())
        {
            dialogueText.text += letter;

            AudioManager.Instance.PlaySFX(10); //Customer

            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void NextLine()
    {
        if (index < lines.Length -1)
        {
            index++;
            dialogueText.text = string.Empty;
            StartCoroutine(WriteLine());
        }

        else
        {
            dialoguePanel.SetActive(false);
            GameManager.Instance.inDialogue = false;
        }
    }

    public void ChangeSkipText()
    {
        if (GameManager.Instance.input.currentControlScheme == "Gamepad")
        {
            skipText.text = "B";
        }

        else
        {
            skipText.text = "Space";
        }
    }
}
