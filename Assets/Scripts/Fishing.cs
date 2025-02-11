using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : MonoBehaviour
{
    public static Fishing Instance; // Singleton pattern

    public GameObject selectedFish;
    public Transform Display1;
    public Transform Display2;
    public Transform Display3;
    public float sale_area = 0;
    private bool isinsalearea = false;
    public GameObject[] fishPrefabs;
    public Transform fishAttachmentPoint;
    public GameObject caughtFish;
    private bool isFishCaught = false;
    public GameObject fishingRod;
    public static bool isFishing = false;
    private CharacterController controller;
    private bool isinfishingArea = false;
    private Animator animator;

    public bool isDisplay1Occupied = false;
    public bool isDisplay2Occupied = false;
    public bool isDisplay3Occupied = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        fishingRod.SetActive(false);
    }

    private void Update()
    {
        if (isinfishingArea && Input.GetMouseButtonDown(0) && caughtFish == null)
        {
            fishingRod.SetActive(true);
            isFishing = true;
            animator.SetBool("isFishing", isFishing);
            animator.SetTrigger("isfishing");
            Debug.Log("Olta at�ld�!");
        }
        if (isinfishingArea && Input.GetMouseButtonDown(1) && caughtFish == null && isFishing)
        {
            animator.SetTrigger("isfishhold");
            Debug.Log("Bal�k tutuldu");
            CatchingFish();
            isFishCaught = true;
        }

        if (isinsalearea && Input.GetKeyDown(KeyCode.E))
        {
            ReleaseFish();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("fisharea"))
        {
            isinfishingArea = true;
            animator.SetBool("isinfishingarea", isinfishingArea);
            Debug.Log("Bal�k tutma b�lgesine girildi.");
        }

        if (other.CompareTag("saleare1") || other.CompareTag("salearea2") || other.CompareTag("salearea3"))
        {
            isinsalearea = true;
            animator.SetBool("isinsalearea", isinsalearea);
            Debug.Log("Sat�� alan�na girildi");
            sale_area = other.CompareTag("saleare1") ? 1 : other.CompareTag("salearea2") ? 2 : 3;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("fisharea"))
        {
            isinfishingArea = false;
            animator.SetBool("isinfishingarea", isinfishingArea);
            Debug.Log("Bal�k tutma b�lgesinden ��k�ld�.");
        }
        if (other.CompareTag("saleare1") || other.CompareTag("salearea2") || other.CompareTag("salearea3"))
        {
            isinsalearea = false;
            animator.SetBool("isinsalearea", isinsalearea);
            Debug.Log("Sat�� alan�ndan ��k�ld�");
        }
    }

    public void EndFishing()
    {
        isFishing = false;
        animator.SetBool("isFishing", isFishing);
        fishingRod.SetActive(false);
    }

    public void CatchingFish()
    {
        int randomIndex = Random.Range(0, fishPrefabs.Length);
        selectedFish = fishPrefabs[randomIndex];
        caughtFish = Instantiate(selectedFish, fishAttachmentPoint.position, fishAttachmentPoint.rotation);
        caughtFish.transform.parent = fishAttachmentPoint;
        Debug.Log("Bal�k tutuldu: " + selectedFish.name);
    }

    public void ReleaseFish()
    {
        if (caughtFish != null)
        {
            if (sale_area == 1 && !isDisplay1Occupied)
            {
                caughtFish.transform.position = Display1.position;
                caughtFish.transform.rotation = Display1.rotation;
                caughtFish.transform.parent = Display1; // Display1'in child'� yap
                isDisplay1Occupied = true;
            }
            else if (sale_area == 2 && !isDisplay2Occupied)
            {
                caughtFish.transform.position = Display2.position;
                caughtFish.transform.rotation = Display2.rotation;
                caughtFish.transform.parent = Display2; // Display2'nin child'� yap
                isDisplay2Occupied = true;
            }
            else if (sale_area == 3 && !isDisplay3Occupied)
            {
                caughtFish.transform.position = Display3.position;
                caughtFish.transform.rotation = Display3.rotation;
                caughtFish.transform.parent = Display3; // Display3'�n child'� yap
                isDisplay3Occupied = true;
            }
            else
            {
                Debug.Log("Bu sat�� alan� zaten dolu!");
                return;
            }

            isFishCaught = false;
            caughtFish = null;
            Debug.Log("Bal�k b�rak�ld�.");
        }
    }
}
