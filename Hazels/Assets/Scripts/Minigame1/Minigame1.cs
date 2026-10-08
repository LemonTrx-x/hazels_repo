using UnityEngine;

public class Minigame1 : MonoBehaviour 
{
    [Header("Caldron")]
    public GameObject cameraPlayer;

    Vector3 initialCameraPos;
    Quaternion initialCameraRot;
    public Vector3 newCameraPos;
    public Quaternion newCameraRot;

    public int progress = 0;

    void Start()
    {
        initialCameraPos = cameraPlayer.transform.position;
        initialCameraRot = cameraPlayer.transform.rotation;

        cameraPlayer.transform.position = newCameraPos;
        cameraPlayer.transform.rotation = newCameraRot;

        GameManager.Instance.inMinigame = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cameraPlayer.transform.position = initialCameraPos;
            cameraPlayer.transform.rotation = initialCameraRot;

            GameManager.Instance.inMinigame = false;
            this.GetComponent<Minigame1>().enabled = false;
        }
    }
}
