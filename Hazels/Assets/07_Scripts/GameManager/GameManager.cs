using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ItemDataBase itemDataBase;

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
        itemDataBase.InitializeDataBase();

        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1;
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
        HandleCursors();
        
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
    }

    public void HandleCursors()
    {
        if (isGamePaused || inMenu)
        {
            if (input.currentControlScheme == "Gamepad")
            {
                virtualMouseUI.SetActive(true);
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = false;
            }

            else
            {
                virtualMouseUI.SetActive(false);
                ResetVirtualMouse();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

        }

        else if (inMinigame)
        {
            virtualMouseUI.SetActive(false);
            ResetVirtualMouse();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        else
        {
            virtualMouseUI.SetActive(false);
            ResetVirtualMouse();
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void ResetVirtualMouse()
    {
        virtualMouseUI.GetComponent<VirtualMouseInput>().cursorTransform.position = new Vector2(Screen.width / 2, Screen.height / 2);
    }
}
