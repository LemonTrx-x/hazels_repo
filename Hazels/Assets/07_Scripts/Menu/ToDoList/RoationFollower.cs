using UnityEngine;

public class RotationFollower : MonoBehaviour
{
    public enum Axis { X, Y, Z }

    public Transform source;

    public Transform target;
    public Axis rotationAxis = Axis.Y;

    public float rotationSpeed = 180f;
    [Tooltip("Tick this box if, due to the model’s orientation, it rotates the wrong way round")]
    public bool invert = false;
    [Tooltip("Minimum rotation per frame from the stickParent to consider it is moving")]
    public float threshold = 0.05f;

    [Header("Smooth")]
    public float accelerationTime = 0.2f;
    public float decelerationTime = 0.6f;

    private float previousYaw; //Previous deviation ("desviación")
    private float currentSpeed; //Degrees per second, with sign

    void Start()
    {
        if (target == null) target = transform;
        previousYaw = source.eulerAngles.y;
    }

    void LateUpdate() //After Minigame1 rotates the stickParent
    {
        if (source == null) return;

        //How much did the stickParent rotate this frame
        float currentYaw = source.eulerAngles.y;
        float delta = Mathf.DeltaAngle(previousYaw, currentYaw);
        previousYaw = currentYaw;

        bool completed = Minigames.Instance != null && 
            Minigames.Instance.progress >= Minigames.Instance.maxProgress;

        //Speed we want to achieve: +speed, -speed or 0 (stop)
        float targetSpeed = 0f;
        if (Mathf.Abs(delta) >= threshold && !completed)
        {
            float direction = Mathf.Sign(delta) * (invert ? -1f : 1f);
            targetSpeed = direction * rotationSpeed;
        }

        //Accelerate if the speed is increasing; brake if it is approaching 0 or change direction
        bool speedingUp = Mathf.Abs(targetSpeed) > Mathf.Abs(currentSpeed) && 
            Mathf.Sign(targetSpeed) == Mathf.Sign(currentSpeed) || currentSpeed == 0f;

        float time = speedingUp ? accelerationTime : decelerationTime;

        //Maximum change in speed per second to achieve rotationSpeed in ‘time’ seconds
        float step = time > 0f ? rotationSpeed / time * Time.deltaTime : Mathf.Infinity;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, step);

        if (currentSpeed != 0f)
        {
            target.Rotate(GetAxis(), currentSpeed * Time.deltaTime, Space.Self);
        }
    }

    private Vector3 GetAxis()
    {
        switch (rotationAxis)
        {
            case Axis.X: return Vector3.right;
            case Axis.Z: return Vector3.forward;
            default: return Vector3.up;
        }
    }
}
