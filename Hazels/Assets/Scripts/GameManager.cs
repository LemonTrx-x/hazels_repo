using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public ItemDataBase itemDataBase;

    public bool inMenu;
    public bool inMinigame;

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
}
