using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ItemDataBase itemDataBase;

    [Header("Booleans")]
    public bool inMenu;
    public bool inMinigame;
    public bool inDialogue;
    public bool inPC;

    public int dayInGame = 0;

    [Header("Player Score & Money")]
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public int coins = 500;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI dayText;
    public PlayerInput input;

    [Header("Pause")]
    public GameObject pauseMenu;
    public GameObject virtualMouseUI;
    public bool isGamePaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        //Initialize the DataBase
        if (itemDataBase != null) itemDataBase.InitializeDataBase();

        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;
    }

    void Start()
    {
        if (itemDataBase != null)
        {
            for (int i = 0; i < itemDataBase.items.Count; i++)
            {
                itemDataBase.items[i].shopStock = 5;
            }
        }

        score = 0;
        coins = 500;
    }

    void Update()
    {
        if (scoreText != null) scoreText.text = "SCORE: " + score.ToString();
        if (coinsText != null) coinsText.text = "COINS: " + coins.ToString();
        if (dayText != null) dayText.text = "DAY: " + dayInGame.ToString();
    }

    public void OnPause(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            if (isGamePaused)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        AudioManager.Instance.PlaySFX(1); //BtnClicked

        HandlePauseUI();
        Time.timeScale = 1;
        isGamePaused = false;
    }

    public void Pause()
    {
        HandlePauseUI();
        Time.timeScale = 0;
        isGamePaused = true;
    }

    public void HandlePauseUI()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }
}
