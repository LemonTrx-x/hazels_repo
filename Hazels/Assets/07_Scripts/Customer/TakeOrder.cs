using UnityEngine;

public class TakeOrder : InteractiveObj
{
    private Customer customer;
    public GameObject dialogueParent;
    public DialogueScript dialogueScript;

    void Start()
    {
        dialogueParent = GameObject.Find("DialogueParent");

        if (dialogueParent != null)
        {
            dialogueScript = dialogueParent.GetComponent<DialogueScript>();
        }
    }
    
    public override void Interact()
    {
        customer = GetComponent<Customer>();

        if (customer.customerOrder.Count == 0)
        {
            customer.GenerateOrder();

            if (customer.customerLines.Length > 0)
            {
                dialogueScript.lines = customer.customerLines;
                dialogueScript.dialoguePanel.SetActive(true);
                dialogueScript.StartDialogue();
            }

            return;
        }

        for (int i = 0; i < customer.hotBarSlots.Count; i++)
        {
            if (customer.hotBarSlots[i].itemData != null)
            {
                customer.CheckOrder();
            }
        }

        if (!customer.finished)
        {
            if (customer.customerLines.Length > 0)
            {
                dialogueScript.lines = customer.customerLines;
                dialogueScript.dialoguePanel.SetActive(true);
                dialogueScript.StartDialogue();
            }
        }

        else
        {
            if (customer.goodFinish == true && customer.goodLines.Length > 0)
            {
                AudioManager.Instance.PlaySFX(11); //GoodOrder

                dialogueScript.lines = customer.goodLines;
                dialogueScript.dialoguePanel.SetActive(true);
                dialogueScript.StartDialogue();
            }

            else if (customer.badFinish == true && customer.badLines.Length > 0)
            {
                AudioManager.Instance.PlaySFX(12); //BadOrder

                dialogueScript.lines = customer.badLines;
                dialogueScript.dialoguePanel.SetActive(true);
                dialogueScript.StartDialogue();
            }
        }
    }
}
