using UnityEngine;

public class Merchant : MonoBehaviour
{
    [Header("Merchant")]
    public GameObject midPosition;
    public GameObject endPosition;
    public int speed = 2;
    public bool goToMid = false;
    public bool finished = false;

    public int price = 20;
    public ItemDataBase itemDataBase;

    void Start()
    {
        goToMid = true;
    }

    void Update()
    {
        MoveToMid();
        MoveToEnd();
    }

    public void OfferItems()
    {

    }

    public void MoveToMid()
    {
        float distance = Vector3.Distance(this.transform.position, midPosition.transform.position);

        if (distance <= 0.01)
        {
            goToMid = false;
        }

        if (goToMid == true)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, midPosition.transform.position, speed * Time.deltaTime);
        }
    }

    public void MoveToEnd()
    {
        if (finished == true)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, endPosition.transform.position, speed * Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndPosition"))
        {
            finished = false;
            Destroy(gameObject);
        }
    }
}
