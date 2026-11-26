using UnityEngine;

/// <summary>
/// Casts a ray along the knife (transform.up). If it hits an object with the cut tag,
/// the object's mesh is sliced at that point, the hole is filled and the cut piece
/// gets a Rigidbody and a MeshCollider.
/// </summary>
public class KnifeCutter : MonoBehaviour
{
    [Header("Detection")]
    [Tooltip("Layers that can be cut. The knife itself must NOT be on these layers.")]
    public LayerMask cuttableLayers = ~0;
    public string cutTag = "CanCut";
    [Tooltip("How far the ray reaches from the knife.")]
    public float rayDistance = 1f;

    [Header("Cut")]
    [Tooltip("Local axis of the knife perpendicular to the blade (blue ray). The cut plane contains the blade.")]
    public Vector3 localCutNormal = Vector3.right;
    [Tooltip("Material for the inside of the cut. If empty, the object's first material is used.")]
    public Material capMaterial;

    [Header("Cut piece")]
    public float pieceMass = 0.2f;
    [Tooltip("Impulse that pushes the piece away from the cut.")]
    public float separationForce = 0.5f;

    [Header("Trigger")]
    public bool cut = false;

    void Update()
    {
        Debug.DrawRay(transform.position, transform.up * rayDistance, Color.red);
        Debug.DrawRay(transform.position, transform.TransformDirection(localCutNormal).normalized * 0.2f, Color.blue);

        if (cut)
        {
            TryCut();
        }
    }

    public bool TryCut()
    {
        if (!Physics.Raycast(transform.position, transform.up, out RaycastHit hit, rayDistance, cuttableLayers, QueryTriggerInteraction.Ignore))
            return false;

        GameObject target = hit.collider.gameObject;
        if (!target.CompareTag(cutTag)) return false;

        Vector3 planeNormal = transform.TransformDirection(localCutNormal).normalized;

        GameObject piece = MeshSlicer.Slice(target, hit.point, planeNormal, capMaterial);
        if (piece == null) return false;

        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.mass = pieceMass;

        // Push the piece away from the part that stayed
        Vector3 fromKept = piece.GetComponent<Renderer>().bounds.center - target.GetComponent<Renderer>().bounds.center;
        Vector3 away = Vector3.Dot(fromKept, planeNormal) >= 0f ? planeNormal : -planeNormal;
        rb.AddForce(away * separationForce, ForceMode.Impulse);

        cut = false;
        return true;
    }
}
