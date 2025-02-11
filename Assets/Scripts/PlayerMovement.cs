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
    public Vector3 cameraOffset = new Vector3(0, 1, -5); // Kamera ile karakter aras�ndaki mesafe
    public float cameraSmoothSpeed = 0.125f; // Kamera hareketinin yumu�akl���

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

        // Bal�k tutma durumunda sadece hareketi engelle, animasyonu engelleme
        if (Fishing.isFishing)
        {
            animator.SetFloat("Speed", 0); // Hareket h�z�n� s�f�rla
            return;
        }

        // Hareket input'lar�n� al
        if (Input.GetKey(KeyCode.W)) moveDirection += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) moveDirection += Vector3.back;
        if (Input.GetKey(KeyCode.A)) moveDirection += Vector3.left;
        if (Input.GetKey(KeyCode.D)) moveDirection += Vector3.right;

        // �apraz hareketler
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) moveDirection = (Vector3.forward + Vector3.right).normalized;
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) moveDirection = (Vector3.forward + Vector3.left).normalized;
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) moveDirection = (Vector3.back + Vector3.right).normalized;
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) moveDirection = (Vector3.back + Vector3.left).normalized;

        // Hareket y�n�ne g�re karakteri d�nd�r
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Karakteri hareket ettir
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // Animator parametresini g�ncelle
        float speed = moveDirection.magnitude;
        animator.SetFloat("Speed", speed);
    }

    void HandleCameraFollow()
    {

        // Kamera i�in hedef pozisyonu belirle
        Vector3 desiredPosition = player.position + cameraOffset;

        // Kamera pozisyonunu yumu�ak bir �ekilde g�ncelle
        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSmoothSpeed);
        cameraTransform.position = smoothedPosition;
        //cameraTransform.position = desiredPosition;

        // Kameray� karaktere do�ru bakacak �ekilde ayarla
        cameraTransform.LookAt(player.position);
    }

    
}












    //public float speed = 3f;
    //public float rotation_speed = 5f;
    //private CharacterController controller;
    //private void Start()
    //{
    //    controller = GetComponent<CharacterController>();//varolan componente m�dahale edebilmek i�in componenti okutuldu
    //}

    //private void Update()
    //{
    //    float horizontal = Input.GetAxis("Horizontal"); // A ,D  ve sol ile sa� ok tu�lar� input al�nan keylerdir
    //    float vertical = Input.GetAxis("Vertical");//W ,S  ve yukar� ile a�a�� ok tu�lar� input al�nan keylerdir

    //    Vector3 movement = transform.forward * vertical + transform.right * horizontal; //d and right keys are positive in the input manager as well as w and up keys/ Claculating how much the player should move
    //    controller.Move(movement*speed*Time.deltaTime);

    //    if (movement.magnitude > 0) //
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(movement); // Hareket y�n�ne g�re hedef d�n��
    //        transform.R
    //        controller.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotation_speed * Time.deltaTime);/*Quaternion.Slerp(transform.rotation, targetRotation, rotation_speed * Time.deltaTime); // Yava��a d�n*/
    //    }
    //}


