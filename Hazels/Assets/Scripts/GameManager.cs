using UnityEngine;
using TMPro;

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
        scoreText.text = "SCORE: " + score.ToString();
        coinsText.text = "COINS: " + coins.ToString();
    }
}
