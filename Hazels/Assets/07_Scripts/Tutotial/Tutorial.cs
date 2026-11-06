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

    public TutorialState currentStateType;

    private Coroutine currentCoroutine;
    private bool isChangingState = false; //Lock to avoid loops in the same frame

    void Start()
    {
        
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
        Debug.Log("Still not Finished");
        yield return null;
    }

    private IEnumerator ClientState()
    {
        Debug.Log("Still not Finished");
        yield return null;
    }

    private IEnumerator BookMenuState()
    {
        Debug.Log("Still not Finished");
        yield return null;
    }
    
    private IEnumerator MaterialsState()
    {
        Debug.Log("Still not Finished");
        yield return null;
    }

    private IEnumerator MinigameState()
    {
        Debug.Log("Still not Finished");
        yield return null;
    }

    private IEnumerator EndOrderState()
    {
        Debug.Log("Still not Finished");
        yield return null;
    }

    private IEnumerator PCState()
    {
        Debug.Log("Still not Finished");
        yield return null;
    }

    private IEnumerator RandomEventsState()
    {
        Debug.Log("Still not Finished");
        yield return null;
    }
}
