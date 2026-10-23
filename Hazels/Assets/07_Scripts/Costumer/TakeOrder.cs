using UnityEngine;

public class TakeOrder : InteractiveObj
{
    private Costumer costumer;
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
        costumer = GetComponent<Costumer>();

        if (costumer.costumerOrder.Count == 0)
        {
            costumer.GenerateOrder();
        }

        for (int i = 0; i < costumer.hotBarSlots.Count; i++)
        {
            if (costumer.hotBarSlots[i].itemData != null)
            {
                costumer.CheckOrder();
            }
        }

        if (!costumer.finished)
        {
            if (costumer.costumerLines.Length > 0)
            {
                dialogueScript.lines = costumer.costumerLines;
                dialogueScript.dialoguePanel.SetActive(true);
                dialogueScript.StartDialogue();
            }
        }

        else
        {
            if (costumer.goodFinish == true && costumer.goodLines.Length > 0)
            {
                dialogueScript.lines = costumer.goodLines;
                dialogueScript.dialoguePanel.SetActive(true);
                dialogueScript.StartDialogue();
            }

            else if (costumer.badFinish == true && costumer.badLines.Length > 0)
            {
                dialogueScript.lines = costumer.badLines;
                dialogueScript.dialoguePanel.SetActive(true);
                dialogueScript.StartDialogue();
            }
        }
    }
}
