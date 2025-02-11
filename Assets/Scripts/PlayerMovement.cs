using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour

{
    
    private Animator animator;
    public Transform player;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform; // Kamera objesi
    public Vector3 cameraOffset = new Vector3(0, 1, -5); // Kamera ile karakter arasýndaki mesafe
    public float cameraSmoothSpeed = 0.125f; // Kamera hareketinin yumuþaklýðý

    private void Start()
    {
        animator = GetComponent<Animator>();

    }
    void Update()
    {
        HandleMovement();
        HandleCameraFollow();

    }

    void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        // Balýk tutma durumunda sadece hareketi engelle, animasyonu engelleme
        if (Fishing.isFishing)
        {
            animator.SetFloat("Speed", 0); // Hareket hýzýný sýfýrla
            return;
        }

        // Hareket input'larýný al
        if (Input.GetKey(KeyCode.W)) moveDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) moveDirection += Vector3.back;
        if (Input.GetKey(KeyCode.A)) moveDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D)) moveDirection += Vector3.right;

        // Çapraz hareketler
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) moveDirection = (Vector3.forward + Vector3.right).normalized;
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) moveDirection = (Vector3.forward + Vector3.left).normalized;
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) moveDirection = (Vector3.back + Vector3.right).normalized;
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) moveDirection = (Vector3.back + Vector3.left).normalized;

        // Hareket yönüne göre karakteri döndür
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Karakteri hareket ettir
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Animator parametresini güncelle
        float speed = moveDirection.magnitude;
        animator.SetFloat("Speed", speed);
    }

    void HandleCameraFollow()
    {

        // Kamera için hedef pozisyonu belirle
        Vector3 desiredPosition = player.position + cameraOffset;

        // Kamera pozisyonunu yumuþak bir þekilde güncelle
        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed);
        cameraTransform.position = smoothedPosition;
        //cameraTransform.position = desiredPosition;

        // Kamerayý karaktere doðru bakacak þekilde ayarla
        cameraTransform.LookAt(player.position);
    }

    
}












    //public float speed = 3f;
    //public float rotation_speed = 5f;
    //private CharacterController controller;
    //private void Start()
    //{
    //    controller = GetComponent<CharacterController>();//varolan componente müdahale edebilmek için componenti okutuldu
    //}

    //private void Update()
    //{
    //    float horizontal = Input.GetAxis("Horizontal"); // A ,D  ve sol ile sað ok tuþlarý input alýnan keylerdir
    //    float vertical = Input.GetAxis("Vertical");//W ,S  ve yukarý ile aþaðý ok tuþlarý input alýnan keylerdir

    //    Vector3 movement = transform.forward * vertical + transform.right * horizontal; //d and right keys are positive in the input manager as well as w and up keys/ Claculating how much the player should move
    //    controller.Move(movement*speed*Time.deltaTime);

    //    if (movement.magnitude > 0) //
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(movement); // Hareket yönüne göre hedef dönüþ
    //        transform.R
    //        controller.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotation_speed * Time.deltaTime);/*Quaternion.Slerp(transform.rotation, targetRotation, rotation_speed * Time.deltaTime); // Yavaþça dön*/
    //    }
    //}


