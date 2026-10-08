using UnityEngine;

public class Minigame1 : MonoBehaviour 
{
    [Header("Caldron")]
    public GameObject cameraPlayer;

    Vector3 initialCameraPos;
    Quaternion initialCameraRot;
    public GameObject newCameraParent;

    public GameObject pointer;
    public GameObject hotBar;

    public int progress = 0;

    void Start()
    {
        initialCameraPos = cameraPlayer.transform.position;
        initialCameraRot = cameraPlayer.transform.rotation;

        cameraPlayer.transform.position = newCameraParent.transform.position;
        cameraPlayer.transform.rotation = newCameraParent.transform.rotation;

        Cursor.lockState = CursorLockMode.None;

        pointer.SetActive(false);
        hotBar.SetActive(false);

        GameManager.Instance.inMenu = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cameraPlayer.transform.position = initialCameraPos;
            cameraPlayer.transform.rotation = initialCameraRot;

            Cursor.lockState = CursorLockMode.Locked;

            pointer.SetActive(true);
            hotBar.SetActive(true);

            GameManager.Instance.inMenu = false;
            this.GetComponent<Minigame1>().enabled = false;
        }
    }
}
