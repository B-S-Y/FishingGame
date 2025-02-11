using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    private Transform[] displayPoints;
    private Transform despawnPoint;
    private NavMeshAgent agent;
    private bool isMovingToDisplay = false;
    private bool hasPickedFish = false; // Balýk alýndý mý?

    public void Init(Transform[] displays, Transform despawn)
    {
        displayPoints = displays;
        despawnPoint = despawn;
        agent = GetComponent<NavMeshAgent>();

        ChooseTarget();
    }

    private void ChooseTarget()
    {
        // Display'lerde balýk var mý kontrol et
        for (int i = 0; i < displayPoints.Length; i++)
        {
            if (displayPoints[i].childCount > 0) // Display'de balýk varsa
            {
                agent.SetDestination(displayPoints[i].position);
                isMovingToDisplay = true;
                hasPickedFish = false; // Balýk henüz alýnmadý
                return;
            }
        }

        // Eðer hiç balýk yoksa despawn noktasýna git
        agent.SetDestination(despawnPoint.position);
        isMovingToDisplay = false;
    }

    private void Update()
    {
        if (isMovingToDisplay && !hasPickedFish && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Display'e ulaþtýðýnda balýðý yok et
            DestroyFish();
            hasPickedFish = true; // Balýk alýndý
            isMovingToDisplay = false;
            agent.SetDestination(despawnPoint.position); // Despawn noktasýna git
        }
        else if (!isMovingToDisplay && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Despawn noktasýna ulaþtýðýnda müþteriyi yok et
            Destroy(gameObject);
        }
    }

    private void DestroyFish()
    {
        // Display'deki balýðý bul ve yok et
        for (int i = 0; i < displayPoints.Length; i++)
        {
            if (displayPoints[i].childCount > 0)
            {
                Transform fish = displayPoints[i].GetChild(0);
                int fishValue = GetFishValue(fish.gameObject);
                ScoreManager.Instance.AddScore(fishValue); // Puaný artýr

                // Display alanýný boþalt
                if (displayPoints[i] == Fishing.Instance.Display1)
                {
                    Fishing.Instance.isDisplay1Occupied = false;
                }
                else if (displayPoints[i] == Fishing.Instance.Display2)
                {
                    Fishing.Instance.isDisplay2Occupied = false;
                }
                else if (displayPoints[i] == Fishing.Instance.Display3)
                {
                    Fishing.Instance.isDisplay3Occupied = false;
                }

                Destroy(fish.gameObject); // Balýðý yok et
                break;
            }
        }
    }

    private int GetFishValue(GameObject fish)
    {
        switch (fish.name)
        {
            case "FishV1(Clone)":
                return 25;
            case "FishV2(Clone)":
                return 50;
            case "FishV3(Clone)":
                return 75;
            case "FishV4(Clone)":
                return 100;
            case "SharkV1(Clone)":
                return 500;
            default:
                return 0;
        }
    }
}