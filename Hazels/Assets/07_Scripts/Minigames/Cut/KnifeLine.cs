using UnityEngine;

public class KnifeLine : MonoBehaviour
{
    public Minigame2 min2;
    
    public GameObject knifeLine;
    public Transform target;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    public void MoveKnifeLine()
    {
        if (target == null) return;

        //Local X axis in world space
        Vector3 axis = transform.right;

        //Distance to the target along the local X axis
        float distance = Vector3.Dot(target.position - transform.position, axis);

        //Current offset from the starting position, along the same axis
        float currentOffset = Vector3.Dot(transform.position - startPosition, axis);
        float desiredOffset = Mathf.Clamp(currentOffset + distance, min2.minOffset, min2.maxOffset);
        distance = desiredOffset - currentOffset;

        Vector3 desiredPosition = transform.position + axis * distance;

        if (min2.smoothSpeed > 0f) transform.position = Vector3.Lerp(transform.position, desiredPosition, min2.smoothSpeed * Time.deltaTime);
        else transform.position = desiredPosition;
    }
}
