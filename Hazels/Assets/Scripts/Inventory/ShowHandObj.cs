using UnityEngine;

public class ShowHandObj : MonoBehaviour
{
    private Slot slot;
    
    [Header("Hands")]
    public Transform hand;
    public GameObject handObj;
    public float objScale = 2f;

    void Start()
    {
        slot = GetComponent<Slot>();
    }

    void Update()
    {
        ShowObj();
    }

    public void ShowObj()
    {
        if (slot.itemData != null && handObj == null)
        {
            GameObject newItemPrefab;

            newItemPrefab = Instantiate(slot.itemData.prefab, hand.position, hand.rotation);

            newItemPrefab.GetComponent<Rigidbody>().useGravity = false;
            newItemPrefab.GetComponent<Rigidbody>().isKinematic = true;
            newItemPrefab.GetComponent<Collider>().isTrigger = true;
            newItemPrefab.transform.localScale /= objScale;
            newItemPrefab.transform.SetParent(hand);

            handObj = newItemPrefab;
        }

        if (slot.itemData == null && handObj != null)
        {
            Destroy(handObj.gameObject);
        }
    }
}
