using UnityEngine;

public class Minigames : MonoBehaviour 
{
    public static Minigames Instance { get; private set; }

    public GameObject cameraPlayer;

    public GameObject newCameraParent;
    public GameObject oldCameraParent;
    public float camSpeed = 5f;

    public GameObject pointer;
    public GameObject hotBar;

    public int progress = 0;

    private bool isMovingToNew = false;
    private bool isMovingToOld = false;

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
        //Moving camera to new position
        if (isMovingToNew)
        {
            cameraPlayer.transform.position = Vector3.Lerp(cameraPlayer.transform.position, newCameraParent.transform.position, camSpeed * Time.deltaTime);
            cameraPlayer.transform.rotation = Quaternion.Lerp(cameraPlayer.transform.rotation, newCameraParent.transform.rotation, camSpeed * Time.deltaTime);

            //If camera is too close, we stop the movement to save recources
            if (Vector3.Distance(cameraPlayer.transform.position, newCameraParent.transform.position) < 0.001f)
            {
                cameraPlayer.transform.position = newCameraParent.transform.position;
                cameraPlayer.transform.rotation = newCameraParent.transform.rotation;
                isMovingToNew = false;
            }
        }

        //Moving camera to original position
        if (isMovingToOld)
        {
            cameraPlayer.transform.position = Vector3.Lerp(cameraPlayer.transform.position, oldCameraParent.transform.position, camSpeed * Time.deltaTime);
            cameraPlayer.transform.rotation = Quaternion.Lerp(cameraPlayer.transform.rotation, oldCameraParent.transform.rotation, camSpeed * Time.deltaTime);

            if (Vector3.Distance(cameraPlayer.transform.position, oldCameraParent.transform.position) < 0.001f)
            {
                cameraPlayer.transform.position = oldCameraParent.transform.position;
                cameraPlayer.transform.rotation = oldCameraParent.transform.rotation;
                isMovingToOld = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            EndMinigame();
        }
    }

    public void StartMiniGame()
    {
        oldCameraParent.transform.rotation = cameraPlayer.transform.rotation;
        
        isMovingToNew = true;
        isMovingToOld = false;

        Cursor.lockState = CursorLockMode.None;

        pointer.SetActive(false);
        hotBar.SetActive(false);

        GameManager.Instance.inMinigame = true;
    }

    public void EndMinigame()
    {
        if (GameManager.Instance.inMinigame)
        {
            isMovingToNew = false;
            isMovingToOld = true;

            Cursor.lockState = CursorLockMode.Locked;

            pointer.SetActive(true);
            hotBar.SetActive(true);

            GameManager.Instance.inMinigame = false;
        }
    }
}
