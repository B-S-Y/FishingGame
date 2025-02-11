using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public Transform despawnPoint;
    public Transform[] displayPoints;

    private void Start()
    {
        StartCoroutine(SpawnCustomer());
    }

    private IEnumerator SpawnCustomer()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f); // 5 saniyede bir müþteri spawnla
            GameObject customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            CustomerAI customerAI = customer.GetComponent<CustomerAI>();
            customerAI.Init(displayPoints, despawnPoint);
        }
    }
}
