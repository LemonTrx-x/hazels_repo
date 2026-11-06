using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public enum TutorialState
    {
        Start, // gameLoop = 0
        Client, // gameLoop = 1
        BookMenu, // gameLoop = 2
        Materials, // gameLoop = 3
        Minigame, // gameLoop = 4
        EndOrder, // gameLoop = 5
        PC, // gameLoop = 6
        RandomEvents // gameLoop = 7
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

    public void CheckGameLoop()
    {
        //Moved
        if (GameManager.Instance.input.actions["Move"].ReadValue<Vector2>().x > 0)
        {
            GameManager.Instance.gameLoop++; // 1   
        }

        // Client finished
        /*
        if ()
        {
            GameManager.Instance.gameLoop++; // 2
        }
        */

        // Book opened
        if (ToDoList.Instance.bookRead)
        {
            GameManager.Instance.gameLoop++; // 3
        }

        // Cupboard opened
        /*
        if ()
        {
            GameManager.Instance.gameLoop++; // 4
        }
        */

        //Potion made
        /*
        if ()
        {
            GameManager.Instance.gameLoop++; // 5
        }
        */

        //Customer has potion (no customers left)
        if (ToDoList.Instance.instCustomer.customersLeft <= 0)
        {
            GameManager.Instance.gameLoop++; // 6
        }

        //PC open
        /*
        if ()
        {
            GameManager.Instance.gameLoop++; // 7
        }
        */
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
