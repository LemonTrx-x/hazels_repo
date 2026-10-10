using UnityEngine;

public class Minigames : MonoBehaviour 
{
    public static Minigames Instance { get; private set; }

    public GameObject cameraPlayer;

    public GameObject newCameraParent;
    public GameObject oldCameraParent;

    public GameObject pointer;
    public GameObject hotBar;

    public int progress = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            EndMinigame();
        }
    }

    public void StartMiniGame()
    {
        cameraPlayer.transform.position = newCameraParent.transform.position;
        cameraPlayer.transform.rotation = newCameraParent.transform.rotation;

        Cursor.lockState = CursorLockMode.None;

        pointer.SetActive(false);
        hotBar.SetActive(false);

        GameManager.Instance.inMinigame = true;
    }

    public void EndMinigame()
    {
        if (GameManager.Instance.inMinigame)
        {
            cameraPlayer.transform.position = oldCameraParent.transform.position;
            cameraPlayer.transform.rotation = oldCameraParent.transform.rotation;

            Cursor.lockState = CursorLockMode.Locked;

            pointer.SetActive(true);
            hotBar.SetActive(true);

            GameManager.Instance.inMinigame = false;
        }
    }
}
