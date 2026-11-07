using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour
{
    public enum TutorialState
    {
        Start,
        Customer, //gameLoop = 1
        BookMenu, //gameLoop = 2
        Materials, //gameLoop = 3
        Minigame, //gameLoop = 4
        EndOrder, //gameLoop = 5
        PC, //gameLoop = 6
        RandomEvents
    }

    public GameObject dialogueParent;
    private DialogueScript dialogueScript;

    public InstantiateCustomer instCustomer;
    public GameObject cupboardInv;
    public GameObject pcInv;

    public TutorialState currentStateType;

    public int gameLoop;

    [Header("Lines")]
    public string[] startLines;
    public string[] customerLines;
    public string[] bookMenuLines;
    public string[] materialsLines;
    public string[] minigameLines;
    public string[] endOrderLines;
    public string[] pcLines;
    public string[] randomEventsLines;

    //Booleans
    private bool dialogueWritten = false;
    private bool moved;
    private bool talked;
    private bool book;
    private bool cupboard;
    private bool potion;
    private bool costumers;
    private bool pc;

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
        CheckGameLoop();
    }

    public void CheckGameLoop()
    {
        //Moved
        if (GameManager.Instance.input.actions["Move"].ReadValue<Vector2>().x != 0 || 
            GameManager.Instance.input.actions["Move"].ReadValue<Vector2>().y != 0)
        {
            if (!moved)
            {
                moved = true;
                gameLoop = 1;
            }
        }

        //Customer finished talking
        if (instCustomer.customer != null)
        {
            if (instCustomer.customer.GetComponent<Customer>().customerOrder.Count > 0)
            {
                if (!GameManager.Instance.inDialogue)
                {
                    if (!talked)
                    {
                        talked = true;
                        gameLoop = 2;
                    }
                }
            }
        }

        //Book opened
        if (ToDoList.Instance.bookRead)
        {
            if (!book)
            {
                book = true;
                gameLoop = 3;
            }
        }

        //Cupboard opened
        if (cupboardInv.activeSelf)
        {
            if (!cupboard)
            {
                cupboard = true;
                gameLoop = 4;
            }
        }
        
        //Potion made NOT WORKING
        if (Minigames.Instance.progress == Minigames.Instance.maxProgress)
        {
            if (!potion)
            {
                potion = true;
                gameLoop = 5;
            }
        }

        if (ToDoList.Instance.currentStateType == ToDoList.PlayerState.EndDay)
        {
            //No customers left NOT WORKING
            if (!GameManager.Instance.inDialogue)
            {
                if (!costumers)
                {
                    costumers = true;
                    gameLoop = 6;
                }
            }

            //PC open NOT WORKING
            if (pcInv.activeSelf)
            {
                if (!pc)
                {
                    pc = true;
                    gameLoop = 7;
                }
            }
        }
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
                yield return StartCoroutine(StartState(startLines));
                break;
            case TutorialState.Customer:
                yield return StartCoroutine(NewLines(customerLines, 1, TutorialState.BookMenu));
                break;
            case TutorialState.BookMenu:
                yield return StartCoroutine(NewLines(bookMenuLines, 2, TutorialState.Materials));
                break;
            case TutorialState.Materials:
                yield return StartCoroutine(NewLines(materialsLines, 3, TutorialState.Minigame));
                break;
            case TutorialState.Minigame:
                yield return StartCoroutine(NewLines(minigameLines, 4, TutorialState.EndOrder));
                break;
            case TutorialState.EndOrder:
                yield return StartCoroutine(NewLines(endOrderLines, 5, TutorialState.PC));
                break;
            case TutorialState.PC:
                yield return StartCoroutine(NewLines(pcLines, 6, TutorialState.RandomEvents));
                break;
            case TutorialState.RandomEvents:
                yield return StartCoroutine(EndState(randomEventsLines));
                break;
        }
    }

    private IEnumerator StartState(string[] lines)
    {
        dialogueScript.lines = lines;
        dialogueScript.dialoguePanel.SetActive(true);
        dialogueScript.StartDialogue();
        yield return null;

        while (true)
        {
            if (moved)
            {
                yield return new WaitForSeconds(1f);
                ChangeState(TutorialState.Customer);
            }
            
            yield return null;
        }
    }

    private IEnumerator NewLines(string[] lines, int gameLoopOrder, TutorialState newState)
    {
        if (gameLoop == gameLoopOrder && dialogueWritten == false)
        {
            dialogueScript.lines = lines;
            dialogueScript.dialoguePanel.SetActive(true);
            dialogueScript.StartDialogue();
            dialogueWritten = true;
            yield return null;
        }

        while (gameLoop == gameLoopOrder)
        {
            yield return null;
        }

        dialogueWritten = false;
        ChangeState(newState);
    }

    private IEnumerator EndState(string[] lines)
    {
        dialogueScript.lines = lines;
        dialogueScript.dialoguePanel.SetActive(true);
        dialogueScript.StartDialogue();
        yield return null;
    }
}
