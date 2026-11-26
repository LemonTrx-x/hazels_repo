using UnityEngine;
using UnityEngine.InputSystem;

public class Minigame2 : MonoBehaviour
{
    public InteractMinigame interactMinigame;
    public KnifeCutter knifeCutterScript;
    
    [Header("Knife")]
    public GameObject knife;
    public GameObject knifeFBX;
    public GameObject knifeNewPos;

    public Transform knifeInitialPos;
    private Quaternion knifeFBXInitialRot;
    private Quaternion knifeFBXNewRot = Quaternion.Euler(0, 90, -90);

    public bool goToNew = false;
    public bool goToOld = false;

    public float moveTime = 1f;

    [Header("References")]
    public Camera cam;

    [Header("Range (relative to the starting position)")]
    public float minOffset = -0.3f;
    public float maxOffset = 0.3f;

    private float targetOffset = 0f; //Current position inside the range (shared by mouse and controller)

    [Header("Smooth Movement")]
    [Tooltip("0 = moves instantly. Higher values = smoother movement.")]
    public float smoothSpeed = 0f;

    [Tooltip("For the movement when using the mouse")]
    private Vector3 origin; //Starting position (centre of the range)
    private Vector3 axis;   //Local X axis of the object in world space

    [Header("Controller")]
    [Tooltip("Units per second the knife moves with the stick.")]
    public float controllerSpeed = 0.5f;

    void Start()
    {
        if (cam == null) cam = Camera.main;

        knifeInitialPos.position = knife.transform.position;
        knifeFBXInitialRot = knifeFBX.transform.rotation;

        origin = knifeNewPos.transform.position;
        axis = knifeNewPos.transform.right;
    }

    void Update()
    {
        if (Minigames.Instance.currentMinigame == interactMinigame.newCurrentMinigame && knife != null)
        {
            ChangeKnifePos();
            
            if (GameManager.Instance.inMinigame)
            {
                MouseMove();
                ControllerMove();
                ApplyMove();

                GetComponentInParent<Collider>().enabled = false;
            }
        }

        if (!GameManager.Instance.inMinigame)
        {
            GetComponentInParent<Collider>().enabled = true;
        }
    }
    
    public void ChangeKnifePos()
    {
        if (goToNew)
        {
            knife.transform.position = Vector3.Lerp(knife.transform.position, knifeNewPos.transform.position, moveTime * Time.deltaTime);
            knifeFBX.transform.rotation = Quaternion.Lerp(knifeFBX.transform.rotation, knifeFBXNewRot, moveTime * Time.deltaTime);

            if (Vector3.Distance(knife.transform.position, knifeNewPos.transform.position) < 0.001f)
            {
                knife.transform.position = knifeNewPos.transform.position;
                knifeFBX.transform.rotation = knifeFBXNewRot;
                goToNew = false;
            }
        }

        if (goToOld)
        {
            knife.transform.position = Vector3.Lerp(knife.transform.position, knifeInitialPos.position, moveTime * Time.deltaTime);
            knifeFBX.transform.rotation = Quaternion.Lerp(knifeFBX.transform.rotation, knifeFBXInitialRot, moveTime * Time.deltaTime);

            if (Vector3.Distance(knife.transform.position, knifeInitialPos.position) < 0.001f)
            {
                knife.transform.position = knifeInitialPos.position;
                knifeFBX.transform.rotation = knifeFBXInitialRot;
                goToOld = false;
            }
        }
    }

    public void MouseMove()
    {
        if (goToNew || goToOld) return;
        if (Mouse.current == null) return;

        //Only take control if the mouse actually moved this frame
        if (Mouse.current.delta.ReadValue().sqrMagnitude < 0.01f) return;

        //Plane that contains the X axis and faces the camera as much as possible
        Vector3 planeNormal = Vector3.ProjectOnPlane(cam.transform.forward, axis);
        if (planeNormal.sqrMagnitude < 0.0001f) return; //Camera looking straight down the axis

        Plane plane = new Plane(planeNormal, origin);
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (!plane.Raycast(ray, out float distance)) return;

        Vector3 hitPoint = ray.GetPoint(distance);

        //How far along the local X axis the mouse is, clamped to the range
        targetOffset = Mathf.Clamp(Vector3.Dot(hitPoint - origin, axis), minOffset, maxOffset);
    }

    public void ControllerMove()
    {
        if (goToNew || goToOld) return;

        float x = GameManager.Instance.input.actions["Move"].ReadValue<Vector2>().x;

        //If the object's X axis points left on screen, invert so stick-right = screen-right
        float screenSign = Vector3.Dot(axis, cam.transform.right) >= 0f ? 1f : -1f;

        targetOffset += x * screenSign * controllerSpeed * Time.deltaTime;
        targetOffset = Mathf.Clamp(targetOffset, minOffset, maxOffset);
    }

    private void ApplyMove()
    {
        if (goToNew || goToOld) return;

        Vector3 targetPos = origin + axis * targetOffset;

        if (smoothSpeed > 0f) knife.transform.position = Vector3.Lerp(knife.transform.position, targetPos, smoothSpeed * Time.deltaTime);
        else knife.transform.position = targetPos;
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

    public void CutMaterials(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed && GameManager.Instance.inMinigame)
        {
            knifeCutterScript.cut = true;
            Minigames.Instance.progress += interactMinigame.newProgressAmount;
        }
    }
}
