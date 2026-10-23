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
            dialogueScript.dialoguePanel.SetActive(true);
        }

        if (costumer.costumerLines.Length > 0)
        {
            dialogueScript.lines = costumer.costumerLines;
            dialogueScript.StartDialogue();
        }

        for (int i = 0; i < costumer.hotBarSlots.Count; i++)
        {
            if (costumer.hotBarSlots[i].itemData != null)
            {
                costumer.CheckOrder();
            }
        }
    }
}
