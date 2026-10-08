using UnityEngine;

public class Stick : MonoBehaviour
{
    public Transform stickParent;
    public bool dragActive;

    public Minigame1 minigame1;

    private Camera mainCamera;
    private float zCoord;
    private Vector3 offset;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.inMinigame)
        {
            dragActive = true;
            //Distance from object to camera to maintain depth
            zCoord = mainCamera.WorldToScreenPoint(gameObject.transform.position).z;
            offset = gameObject.transform.position - GetMouseAsWorldPoint();
        }
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        //Takes the mouse position on screen and turns it in 3D world coordinates
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = zCoord;
        return mainCamera.ScreenToWorldPoint(mousePos);
    }

    void OnMouseDrag()
    {
        if (GameManager.Instance.inMinigame)
        {
            if (dragActive)
            {
                Vector3 targetPosition = GetMouseAsWorldPoint() + offset;
                this.transform.position = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);
            }
        }
    }

    void OnMouseUp()
    {
        if (GameManager.Instance.inMinigame)
        {
            dragActive = false;
        }
    }
}
