using TMPro;
using UnityEngine;

public class InstantiateCustomer : MonoBehaviour
{
    public Transform spawn;
    public GameObject customer = null;
    public GameObject customerPrefab;
    public GameObject merchantPrefab;
    public float specialChance = 20f;
    float chance;

    public int customersLeft = 1;
    public TextMeshProUGUI customersLeftText;

    public GameObject hotBarPlayer;
    public GameObject midPos;
    public GameObject endPos;

    void Start()
    {
        customersLeftText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (customer == null && customersLeft > 0)
        {
            chance = Random.value;
            GameObject prefabToUse = chance <= (specialChance / 100) ? merchantPrefab : customerPrefab;

            OnInstantiateCustomer(prefabToUse);
        }
    }

    public void OnInstantiateCustomer(GameObject prefab)
    {
        GameObject newCustomer;

        newCustomer = Instantiate(prefab, spawn.position, spawn.rotation);

        if (chance <= (specialChance / 100))
        {
            newCustomer.GetComponent<Merchant>().midPosition = midPos;
            newCustomer.GetComponent<Merchant>().endPosition = endPos;
        }

        else
        {
            newCustomer.GetComponent<Customer>().hotBarSlots.AddRange(hotBarPlayer.GetComponentsInChildren<Slot>());
            newCustomer.GetComponent<Customer>().midPosition = midPos;
            newCustomer.GetComponent<Customer>().endPosition = endPos;
        }

        customer = newCustomer;
        customersLeft--;

        customersLeftText.text = "Customers left: " + customersLeft.ToString();
        customersLeftText.gameObject.SetActive(true);
    }
}
