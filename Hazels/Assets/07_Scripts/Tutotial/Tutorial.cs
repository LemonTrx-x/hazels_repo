using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public enum TutorialState
    {
        Start,
        Client,
        BookMenu,
        Materials,
        Minigame,
        EndOrder,
        PC,
        RandomEvents
    }

    public GameObject dialogueParent;
    private DialogueScript dialogueScript;

    public TutorialState currentStateType;

    [Header("Lines")]
    public string[] startLines;
    public string[] clientLines;
    public string[] bookMenuLines;
    public string[] materialsLines;
    public string[] minigameLines;
    public string[] endOrderLines;
    public string[] pcLines;
    public string[] randomEventsLines;

    private Coroutine currentCoroutine;
    private bool isChangingState = false; //Lock to avoid loops in the same frame

    void Start()
    {
        dialogueParent = GameObject.Find("DialogueParent");

        if (dialogueParent != null)
        {
            dialogueScript = dialogueParent.GetComponent<DialogueScript>();
        }

        ChangeState(TutorialState.Start);
    }

    void Update()
    {
        
    }

    public void ChangeState(TutorialState newState)
    {
        //If already changing state at this exact moment, block recursive call to avoid StackOverflow
        if (isChangingState)
        {
            return;
        }

        //If the same state, return
        if (currentStateType == newState && currentCoroutine != null)
        {
            return;
        }

        isChangingState = true; //Activate lock

        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        currentStateType = newState;
        currentCoroutine = StartCoroutine(GetStateEnumerator(newState));

        isChangingState = false; //Deactivate lock when finished
    }

    private IEnumerator GetStateEnumerator(TutorialState state)
    {
        switch (state)
        {
            case TutorialState.Start:
                yield return StartCoroutine(StartState());
                break;
            case TutorialState.Client:
                yield return StartCoroutine(ClientState());
                break;
            case TutorialState.BookMenu:
                yield return StartCoroutine(BookMenuState());
                break;
            case TutorialState.Materials:
                yield return StartCoroutine(MaterialsState());
                break;
            case TutorialState.Minigame:
                yield return StartCoroutine(MinigameState());
                break;
            case TutorialState.EndOrder:
                yield return StartCoroutine(EndOrderState());
                break;
            case TutorialState.PC:
                yield return StartCoroutine(PCState());
                break;
            case TutorialState.RandomEvents:
                yield return StartCoroutine(RandomEventsState());
                break;
        }
    }

    private IEnumerator StartState()
    {
        dialogueScript.lines = startLines;
        yield return null;
        dialogueScript.dialoguePanel.SetActive(true);
        dialogueScript.StartDialogue();
    }

    private IEnumerator ClientState()
    {
        dialogueScript.lines = clientLines;
        yield return null;
        dialogueScript.dialoguePanel.SetActive(true);
        dialogueScript.StartDialogue();
    }

    private IEnumerator BookMenuState()
    {
        dialogueScript.lines = bookMenuLines;
        yield return null;
        dialogueScript.dialoguePanel.SetActive(true);
        dialogueScript.StartDialogue();
    }
    
    private IEnumerator MaterialsState()
    {
        dialogueScript.lines = materialsLines;
        yield return null;
        dialogueScript.dialoguePanel.SetActive(true);
        dialogueScript.StartDialogue();
    }

    private IEnumerator MinigameState()
    {
        dialogueScript.lines = minigameLines;
        yield return null;
        dialogueScript.dialoguePanel.SetActive(true);
        dialogueScript.StartDialogue();
    }

    private IEnumerator EndOrderState()
    {
        dialogueScript.lines = endOrderLines;
        yield return null;
        dialogueScript.dialoguePanel.SetActive(true);
        dialogueScript.StartDialogue();
    }

    private IEnumerator PCState()
    {
        dialogueScript.lines = pcLines;
        yield return null;
        dialogueScript.dialoguePanel.SetActive(true);
        dialogueScript.StartDialogue();
    }

    private IEnumerator RandomEventsState()
    {
        dialogueScript.lines = randomEventsLines;
        yield return null;
        dialogueScript.dialoguePanel.SetActive(true);
        dialogueScript.StartDialogue();
    }
}
