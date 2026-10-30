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
        FinishOrder,
        EndDay
    }

    [Header("Config")]
    public TextMeshProUGUI toDoListText;
    public InstantiateCostumer instCostumer;
    public Costumer costumer;
    public PlayerState currentStateType;

    [Header("Step Guide")]
    public GameObject stepGuide;
    public GameObject pos1;
    public GameObject pos2;
    public float amplitude = 0.1f;
    public float speed = 1f;
    private Vector3 initialPosition;

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
        initialPosition = stepGuide.transform.position;
        stepGuide.SetActive(false);

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

        LevitateObj();
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
            case PlayerState.FinishOrder:
                yield return StartCoroutine(FinishOrderState());
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

        if (stepGuide != null && pos1 != null)
        {
            stepGuide.SetActive(true);
            stepGuide.transform.position = pos1.transform.position;
            stepGuide.transform.rotation = pos1.transform.rotation;
        }

        while (true)
        {
            //Make sure costumer exists, has an order and hasn't finished yet
            if (costumer != null)
            {
                if (costumer.costumerOrder != null)
                {
                    if (costumer.costumerOrder.Count > 0)
                    {
                        if (!costumer.finished)
                        {
                            ChangeState(PlayerState.InOrder);
                            yield break;
                        }
                    }
                }
            }

            yield return null;
        }
    }

    private IEnumerator InOrderState()
    {
        toDoListText.text = "Make " + costumer.potionsNames;
        yield return null;

        if (stepGuide != null && pos2 != null)
        {
            stepGuide.transform.position = pos2.transform.position;
            stepGuide.transform.rotation = pos2.transform.rotation;
        }

        //Wait until current costumer finishes order
        while (costumer != null && !costumer.finished)
        {
            yield return null;
        }

        ChangeState(PlayerState.FinishOrder);
    }

    private IEnumerator FinishOrderState()
    {
        toDoListText.text = "Wait for the next costumer.";
        yield return new WaitForSeconds(1f);

        if (stepGuide != null)
        {
            stepGuide.SetActive(false);
        }

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

    void LevitateObj()
    {
        float newY = initialPosition.y + Mathf.Sin(Time.time * speed) * amplitude;
        stepGuide.transform.position = new Vector3(stepGuide.transform.position.x, newY, stepGuide.transform.position.z);
    }
}
