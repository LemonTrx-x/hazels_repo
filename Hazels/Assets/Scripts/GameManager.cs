using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ItemDataBase itemDataBase;

    public bool inMenu;
    public bool inMinigame;

    public int dayInGame = 0;

    [Header("Player Score & Money")]
    public int score = 0;
    public TextMeshProUGUI scoreText;
    public int coins = 500;
    public TextMeshProUGUI coinsText;

    [Header("Pause")]
    public GameObject pauseMenu;
    public GameObject pauseIcon;
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
        itemDataBase.InitializeDataBase();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        for (int i = 0; i < itemDataBase.items.Count; i++)
        {
            itemDataBase.items[i].shopStock = 5;
        }

        score = 0;
        coins = 500;
    }

    void Update()
    {
        HandleCursor();
        
        scoreText.text = "SCORE: " + score.ToString();
        coinsText.text = "COINS: " + coins.ToString();
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
        pauseIcon.SetActive(!pauseIcon.activeSelf);
    }

    public void HandleCursor()
    {
        if (isGamePaused || inMenu || inMinigame)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
