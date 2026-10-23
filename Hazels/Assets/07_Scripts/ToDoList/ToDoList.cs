using System.Collections;
using TMPro;
using UnityEngine;

public class ToDoList : MonoBehaviour
{
    public static ToDoList Instance { get; private set; }

    public enum PlayerState
    {
        StartDay,
        TakeOrder,
        InOrder,
        EndDay
    }

    [Header("Config")]
    public TextMeshProUGUI toDoListText;
    public InstantiateCostumer instCostumer;
    public Costumer costumer;
    private PlayerState currentStateType;

    private Coroutine currentCoroutine;
    private bool isChangingState = false; //Lock to avoid loops in the same frame

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        ChangeState(PlayerState.StartDay);
    }

    void Update()
    {
        if (instCostumer != null && instCostumer.costumer != null)
        {
            costumer = instCostumer.costumer.GetComponent<Costumer>();
        }

        else
        {
            costumer = null;
        }
    }

    public void ChangeState(PlayerState newState)
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

    private IEnumerator GetStateEnumerator(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.StartDay:
                yield return StartCoroutine(StartDayState());
                break;
            case PlayerState.TakeOrder:
                yield return StartCoroutine(TakeOrderState());
                break;
            case PlayerState.InOrder:
                yield return StartCoroutine(InOrderState());
                break;
            case PlayerState.EndDay:
                yield return StartCoroutine(EndDayState());
                break;
        }
    }

    private IEnumerator StartDayState()
    {
        toDoListText.text = "Day started!";
        yield return new WaitForSeconds(2f);
        ChangeState(PlayerState.TakeOrder);
    }

    private IEnumerator TakeOrderState()
    {
        toDoListText.text = "Take the costumer's order.";
        yield return null;

        while (true)
        {
            //Make sure costumer exists, has an order and hasn't finished yet
            if (costumer != null && costumer.costumerOrder != null && costumer.costumerOrder.Count > 0 && !costumer.finished)
            {
                ChangeState(PlayerState.InOrder);
                yield break;
            }

            yield return null;
        }
    }

    private IEnumerator InOrderState()
    {
        toDoListText.text = "Make " + costumer.potionsNames;
        yield return null;

        //Wait until current costumer finishes order
        while (costumer != null && !costumer.finished)
        {
            yield return null;
        }

        //Wait until old costumer dissapears from InstantiateCostumer
        //(It avoids reading the old costumer)
        while (instCostumer != null && instCostumer.costumer != null)
        {
            yield return null;
        }

        //Clean the reference for the next costumers
        costumer = null;

        //Return to TakeOrder to wait for next costumer, if there are any left
        if (instCostumer.costumersLeft > 0)
        {
            ChangeState(PlayerState.TakeOrder);
        }

        else
        {
            ChangeState(PlayerState.EndDay);
        }
    }

    private IEnumerator EndDayState()
    {
        toDoListText.text = "Day finished! Go to the PC to buy new materials for tomorrow.";
        yield return null;
    }
}
