using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{

    public float runSpeed = 10;
    public float walkSpeed = 5;
    private float moveSpeed = 45f;
    public float jumpForce = 10f;
    public float lookSensitivity = 3f;

    public AudioSource footstep;
    private float timer;
    private float maxTimer = 1f;

    [SerializeField] private bool isGrounded;

    private Rigidbody rb;
    private Camera playerCamera;
    private float xRotation = 0f;
    private float move_X;
    private float move_Z;
    private float maxSpeed = 8;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public GameObject prefab;
    private bool ingame = false;
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            moveSpeed = runSpeed;
        }else{
            moveSpeed = walkSpeed;
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
        else{
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (ingame) {
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            ingame = !ingame;
            if (ingame) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput != 0 && timer <= 0)
        {
            //footstep.Play();
            timer = maxTimer;
        }

        if (verticalInput != 0 && timer <= 0)
        {
            //footstep.Play();
            timer = maxTimer;
        }
        timer -= Time.deltaTime;

        Vector3 movement = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;

        if (rb.velocity.magnitude < maxSpeed)
        {
            rb.AddForce(movement * (moveSpeed * 50 * Time.deltaTime));
        }
        
        //rb.velocity = (movement * (moveSpeed * 50 * Time.fixedDeltaTime)) + new Vector3(0f, rb.velocity.y, 0f);
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

}
