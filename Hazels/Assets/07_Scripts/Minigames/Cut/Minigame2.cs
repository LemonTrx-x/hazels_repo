using UnityEngine;
using UnityEngine.InputSystem;

public class Minigame2 : MonoBehaviour
{
    public InteractMinigame interactMinigame;
    
    [Header("Knife")]
    public GameObject knife;
    public GameObject knifeFBX;
    public GameObject knifeNewPos;

    private Vector3 knifeInitialPos;
    private Quaternion knifeFBXInitialRot;
    private Quaternion knifeFBXNewRot = Quaternion.Euler(0, 90, -90);

    public float moveTime = 1f;

    [Header("References")]
    public Camera cam;

    [Header("Range (relative to the starting position)")]
    public float minOffset = -0.3f;
    public float maxOffset = 0.3f;

    [Header("Movement")]
    [Tooltip("0 = moves instantly. Higher values = smoother movement.")]
    public float smoothSpeed = 0f;

    private Vector3 origin; // Starting position (centre of the range)
    private Vector3 axis;   // Local X axis of the object in world space

    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance.inMinigame && 
            knife != null && 
            Minigames.Instance.currentMinigame == interactMinigame.newCurrentMinigame)
        {
            ChangeKnifePos();
            MouseMove();
            ControllerMove();
        }
    }
    
    public void ChangeKnifePos()
    {
        if (Minigames.Instance.isMovingToNew)
        {
            knifeInitialPos = knife.transform.position;
            knife.transform.position = Vector3.Lerp(knife.transform.position, knifeNewPos.transform.position, moveTime * Time.deltaTime);

            knifeFBXInitialRot = knifeFBX.transform.rotation;
            knifeFBX.transform.rotation = Quaternion.Lerp(knifeFBX.transform.rotation, knifeFBXNewRot, moveTime * Time.deltaTime);

            if (Vector3.Distance(knife.transform.position, knifeNewPos.transform.position) < 0.001f)
            {
                knife.transform.position = knifeNewPos.transform.position;
                knifeFBX.transform.rotation = knifeFBXNewRot;
            }
        }

        if (Minigames.Instance.isMovingToOld)
        {
            knife.transform.position = Vector3.Lerp(knife.transform.position, knifeInitialPos, moveTime * Time.deltaTime);
            knifeFBX.transform.rotation = Quaternion.Lerp(knifeFBX.transform.rotation, knifeFBXInitialRot, moveTime * Time.deltaTime);

            if (Vector3.Distance(knife.transform.position, knifeInitialPos) < 0.001f)
            {
                knife.transform.position = knifeInitialPos;
                knifeFBX.transform.rotation = knifeFBXInitialRot;
            }
        }
    }

    public void MouseMove()
    {
        if (Minigames.Instance.isMovingToNew == false && Minigames.Instance.isMovingToOld == false)
        {
            origin = knifeNewPos.transform.position;
            axis = knifeNewPos.transform.right;

            //Plane that contains the X axis and faces the camera as much as possible
            Vector3 planeNormal = Vector3.ProjectOnPlane(cam.transform.forward, axis);
            if (planeNormal.sqrMagnitude < 0.0001f) return; //Camera looking straight down the axis

            Plane plane = new Plane(planeNormal, origin);
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!plane.Raycast(ray, out float distance)) return;

            Vector3 hitPoint = ray.GetPoint(distance);

            //How far along the local X axis the mouse is, clamped to the range
            float offset = Vector3.Dot(hitPoint - origin, axis);
            offset = Mathf.Clamp(offset, minOffset, maxOffset);

            Vector3 targetPos = origin + axis * offset;

            if (smoothSpeed > 0f) knife.transform.position = Vector3.Lerp(knife.transform.position, targetPos, smoothSpeed * Time.deltaTime);
            else knife.transform.position = targetPos;
        }
    }

    //Draws the range in the Scene view when the object is selected
    void OnDrawGizmosSelected()
    {
        Vector3 o = Application.isPlaying ? origin : knifeNewPos.transform.position;
        Vector3 a = Application.isPlaying ? axis : knifeNewPos.transform.right;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(o + a * minOffset, o + a * maxOffset);
        Gizmos.DrawWireSphere(o + a * minOffset, 0.05f);
        Gizmos.DrawWireSphere(o + a * maxOffset, 0.05f);
    }

    public void ControllerMove()
    {
        float x = GameManager.Instance.input.actions["Move"].ReadValue<Vector2>().x;
    }

    public void CutMaterials(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && GameManager.Instance.inMinigame)
        {
            Debug.Log("Lets cut this thingy!");
        }
    }
}
