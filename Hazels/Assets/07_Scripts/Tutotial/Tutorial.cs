using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public enum TutorialState
    {
        Start,
        Customer,
        BookMenu,
        Materials,
        Minigame,
        EndOrder,
        PC,
        RandomEvents,
        Finished
    }

    public GameObject dialogueParent;
    private DialogueScript dialogueScript;

    public InstantiateCustomer instCustomer;
    public GameObject cupboardInv;
    public GameObject pcInv;

    public TutorialState currentStateType;

    [Header("Lines")]
    public string[] startLines;
    public string[] customerLines;
    public string[] bookMenuLines;
    public string[] materialsLines;
    public string[] minigameLines;
    public string[] endOrderLines;
    public string[] pcLines;
    public string[] randomEventsLines;

    void Start()
    {
        if (dialogueParent == null)
        {
            dialogueParent = GameObject.Find("DialogueParent");
        }

        if (dialogueParent != null)
        {
            dialogueScript = dialogueParent.GetComponent<DialogueScript>();
        }

        if (dialogueScript == null)
        {
            Debug.LogError("Tutorial: no se encontró DialogueScript.");
            return;
        }

        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence()
    {
        //Start, wait until de player moves
        SetState(TutorialState.Start, startLines);
        yield return new WaitUntil(PlayerMoved);
        yield return new WaitForSeconds(1f);

        //Costumer, wait for the costumer to stop talking
        SetState(TutorialState.Customer, customerLines);
        yield return new WaitUntil(CustomerFinishedTalking);

        //BookMenu, wait for the player to read it
        SetState(TutorialState.BookMenu, bookMenuLines);
        yield return new WaitUntil(() => ToDoList.Instance.bookRead);

        //Materials, wit for the cupboard to open
        SetState(TutorialState.Materials, materialsLines);
        yield return new WaitUntil(() => cupboardInv.activeInHierarchy);

        //Minigame, wait until the potion is done
        SetState(TutorialState.Minigame, minigameLines);
        yield return new WaitUntil(PotionMade);

        //EndOrder, wait until there are no customers left
        SetState(TutorialState.EndOrder, endOrderLines);
        yield return new WaitUntil(NoCustomersLeft);

        //PC, wait for PC to open
        SetState(TutorialState.PC, pcLines);
        yield return new WaitUntil(() => pcInv.activeInHierarchy);

        //RandomEvents
        SetState(TutorialState.RandomEvents, randomEventsLines);

        currentStateType = TutorialState.Finished;
    }

    private void SetState(TutorialState state, string[] lines)
    {
        currentStateType = state;
        Debug.Log("Tutorial: " + state);

        if (lines == null || lines.Length == 0) return;

        dialogueScript.lines = lines;
        dialogueScript.dialoguePanel.SetActive(true);
        dialogueScript.StartDialogue();
    }

    //==Conditions==

    private bool PlayerMoved()
    {
        return GameManager.Instance.input.actions["Move"].ReadValue<Vector2>().sqrMagnitude > 0.01f;
    }

    private bool CustomerFinishedTalking()
    {
        if (instCustomer.customer == null) return false;

        Customer c = instCustomer.customer.GetComponent<Customer>();
        return c != null && 
            c.customerOrder.Count > 0 && 
            !GameManager.Instance.inDialogue;
    }

    private bool PotionMade()
    {
        return Minigames.Instance != null && 
            Minigames.Instance.potionMade == true;
    }

    private bool NoCustomersLeft()
    {
        return ToDoList.Instance.currentStateType == ToDoList.PlayerState.EndDay &&
            !GameManager.Instance.inDialogue;
    }
}
