using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    private Transform[] displayPoints;
    private Transform despawnPoint;
    private NavMeshAgent agent;
    private bool isMovingToDisplay = false;
    private bool hasPickedFish = false; // Bal�k al�nd� m�?

    public void Init(Transform[] displays, Transform despawn)
    {
        displayPoints = displays;
        despawnPoint = despawn;
        agent = GetComponent<NavMeshAgent>();

        ChooseTarget();
    }

    private void ChooseTarget()
    {
        // Display'lerde bal�k var m� kontrol et
        for (int i = 0; i < displayPoints.Length; i++)
        {
            if (displayPoints[i].childCount > 0) // Display'de bal�k varsa
            {
                agent.SetDestination(displayPoints[i].position);
                isMovingToDisplay = true;
                hasPickedFish = false; // Bal�k hen�z al�nmad�
                return;
            }
        }

        // E�er hi� bal�k yoksa despawn noktas�na git
        agent.SetDestination(despawnPoint.position);
        isMovingToDisplay = false;
    }

    private void Update()
    {
        if (isMovingToDisplay && !hasPickedFish && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Display'e ula�t���nda bal��� yok et
            DestroyFish();
            hasPickedFish = true; // Bal�k al�nd�
            isMovingToDisplay = false;
            agent.SetDestination(despawnPoint.position); // Despawn noktas�na git
        }
        else if (!isMovingToDisplay && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Despawn noktas�na ula�t���nda m��teriyi yok et
            Destroy(gameObject);
        }
    }

    private void DestroyFish()
    {
        // Display'deki bal��� bul ve yok et
        for (int i = 0; i < displayPoints.Length; i++)
        {
            if (displayPoints[i].childCount > 0)
            {
                Transform fish = displayPoints[i].GetChild(0);
                int fishValue = GetFishValue(fish.gameObject);
                ScoreManager.Instance.AddScore(fishValue); // Puan� art�r

                // Display alan�n� bo�alt
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

                Destroy(fish.gameObject); // Bal��� yok et
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