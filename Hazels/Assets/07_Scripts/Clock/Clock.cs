using UnityEngine;

public class Clock : MonoBehaviour
{
    public enum HandType
    {
        Hour,
        Minute
    }

    [System.Serializable]
    public class ClockHand
    {
        public Transform hand;
        [Tooltip("Hour: advances one hour mark (30°) per customer. Minute: one full turn (360°) per customer")]
        public HandType type = HandType.Minute;
        [HideInInspector] public Quaternion startRotation;
    }

    public InstantiateCustomer instCustomer;
    float totalCustomers = 0f;
    public ClockHand[] hands;

    [Tooltip("Animation speed of the minute hand in degrees per second")]
    public float rotationSpeed = 180f;
    public bool clockwise = true;
    [Tooltip("Local rotation axis. UI/2D: (0,0,1)")]
    public Vector3 rotationAxis = Vector3.forward;

    int customersServed = 0;
    GameObject trackedCustomer = null; //Customer currently being tracked
    float currentProgress = 0f; //Customers served, already animated (can be fractional)

    void Start()
    {
        totalCustomers = instCustomer.customersLeft;
        
        foreach (var h in hands)
        {
            if (h.hand != null)
            {
                h.startRotation = h.hand.localRotation;
            }
        }
    }

    void Update()
    {
        if (instCustomer == null) return;

        TrackCustomers();

        //Smoothly move towards the number of customers served
        //(RotationSpeed / 360 = customers per second, since the minute hand turns 360° per customer)
        currentProgress = Mathf.MoveTowards(currentProgress, customersServed, rotationSpeed / 360f * Time.deltaTime);

        float dir = clockwise ? 1f : -1f;

        foreach (var h in hands)
        {
            if (h.hand == null) continue;

            //Each customer = 1 hour
            float degreesPerCustomer = h.type == HandType.Hour ? 360f / totalCustomers : 360f;
            float angle = dir * degreesPerCustomer * currentProgress;

            h.hand.localRotation = h.startRotation * Quaternion.AngleAxis(angle, rotationAxis);
        }
    }

    void TrackCustomers()
    {
        GameObject current = instCustomer.customer;

        // Is the tracked customer gone?
        if (!ReferenceEquals(trackedCustomer, null))
        {
            bool destroyed = trackedCustomer == null; //Destroyed (Unity null)
            bool replaced  = !ReferenceEquals(current, trackedCustomer); //Different customer has spawned

            if (destroyed || replaced)
            {
                customersServed++;
                trackedCustomer = null;
            }
        }

        //Start tracking the new customer
        if (ReferenceEquals(trackedCustomer, null) && current != null)
            trackedCustomer = current;
    }
}
