using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ItemDataBase itemDataBase;

    public bool inMenu;
    public bool inMinigame;

    public int dayInGame = 0;

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
    }
}
