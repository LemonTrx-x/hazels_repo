using UnityEngine;

public class Minigame1 : MonoBehaviour
{
    [Header("Stick")]
    public Transform stickParent;
    public bool dragActive;
    public float rotationSpeed = 15f; //Smooth speed

    private Camera mainCamera;
    private Plane rotationPlane;
    private Quaternion initialParentRotation;
    private float initialMouseAngle;

    public float degreesNeeded = 360f; //Degrees needed to +1
    private float accumulatedAngle = 0f; //Keep track of the degrees completed
    private float previousMouseAngle; //Previous frame angle

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.inMinigame && stickParent != null)
        {
            dragActive = true;

            //Create a horizontal plane that passes through the position of stickParent
            rotationPlane = new Plane(Vector3.up, stickParent.position);

            //Obtain the point on the globe where the mouse touches the plane
            if (GetMouseWorldPositionOnPlane(out Vector3 mouseWorldPos))
            {
                Vector3 dir = mouseWorldPos - stickParent.position;
                initialMouseAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

                //Initialise previousMouseAngle with the current angle when the mouse is clicked
                previousMouseAngle = initialMouseAngle;

                initialParentRotation = stickParent.rotation;
            }
        }
    }

    void OnMouseDrag()
    {
        if (GameManager.Instance.inMinigame && dragActive && stickParent != null)
        {
            if (GetMouseWorldPositionOnPlane(out Vector3 mouseWorldPos))
            {
                Vector3 dir = mouseWorldPos - stickParent.position;

                //If the cursor is positioned directly over the pivot point, we avoid abrupt calculations
                if (dir.sqrMagnitude < 0.001f)
                {
                    return;
                }

                float currentMouseAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

                //Calculate the angle difference between this frame and the previous one
                float deltaAngle = Mathf.DeltaAngle(previousMouseAngle, currentMouseAngle);

                //Sum the absolute displacement
                accumulatedAngle += Mathf.Abs(deltaAngle);
                previousMouseAngle = currentMouseAngle;

                //Increment the integer each time the degreesNeeded is reached
                if (accumulatedAngle >= degreesNeeded)
                {
                    Minigames.Instance.progress++;
                    accumulatedAngle -= degreesNeeded;
                    Debug.Log("Total progress: " + Minigames.Instance.progress);
                }

                //Work out how much the mouse has rotated since we clicked
                float angleDelta = currentMouseAngle - initialMouseAngle;

                //Apply relative rotation based on the initial orientation
                Quaternion targetRotation = initialParentRotation * Quaternion.Euler(0f, angleDelta, 0f);

                //Smooth out the movement to prevent fast turns
                stickParent.rotation = Quaternion.Slerp(stickParent.rotation, targetRotation, Time.deltaTime * rotationSpeed);
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

    //Draw a ray from the camera to the XZ plane of the pivot
    private bool GetMouseWorldPositionOnPlane(out Vector3 worldPoint)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (rotationPlane.Raycast(ray, out float enter))
        {
            worldPoint = ray.GetPoint(enter);
            return true;
        }
        worldPoint = Vector3.zero;
        return false;
    }
}