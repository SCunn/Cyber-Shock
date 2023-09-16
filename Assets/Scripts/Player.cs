using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 12.5f, runSpeed = 25f;

    // Adding gravity
    public Vector3 velocity;
    public float gravityModifier;


    public CharacterController myController;
    public Transform myCameraHead;

    public float mouseSensitivity = 100f;
    private float cameraVerticalRotation;

    public GameObject bullet;
    public Transform firePosition;

    public GameObject muzzleFlash, bulletHole, waterLeak, impactDebris;
    
    // Animations
    public Animator myAnimator;
    
    // Jumping
    public float jumpHeight = 10f;
    private bool readyToJump;
    public Transform ground;
    public LayerMask groundLayer;
    public float groundDistance = 0.5f;

    // Crouching
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 bodyScale;
    public Transform myBody;
    private float initialControllerHeight;
    public float crouchSpeed = 6f;
    private bool isCrouching = false;

    // Sliding
    public bool isRunning = false, startSliderTimer;
    public float currentSlideTimer, maxSlideTime = 2f;
    public float slideSpeed =  20f;

    // Start is called before the first frame update
    void Start()
    {
        bodyScale = myBody.localScale;
        initialControllerHeight = myController.height;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        CameraMovement();
        Jump();
        Shoot();
        Crouching();
        SlideCounter();

    }

    private void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.C))
            StartCrouching();

        if (Input.GetKeyUp(KeyCode.C)|| currentSlideTimer > maxSlideTime)
            StopCrouching();

    }


    private void StartCrouching()
    {
        myBody.localScale = crouchScale;
        myCameraHead.position -= new Vector3 (0, 1f, 0);

        myController.height /= 2;
        isCrouching = true;

        if (isRunning)
        {
            velocity = Vector3.ProjectOnPlane(myCameraHead.transform.forward, Vector3.up).normalized * slideSpeed * Time.deltaTime;
            startSliderTimer = true;
        }
    }

    private void StopCrouching()
    {
        currentSlideTimer = 0f;
        velocity = new Vector3(0f, 0f, 0f);
        startSliderTimer = false;

        myBody.localScale = bodyScale;
        myCameraHead.position += new Vector3(0, 1f, 0);

        myController.height = initialControllerHeight;
        isCrouching = false;
    }


    void Jump()
    {
        readyToJump = Physics.OverlapSphere(ground.position, groundDistance, groundLayer).Length > 0;

        if (Input.GetButtonDown("Jump") && readyToJump)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y) * Time.deltaTime;
        }

        myController.Move(velocity);
    }

    private void Shoot()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if(Physics.Raycast(myCameraHead.position, myCameraHead.forward, out hit, 100f))
            {
                //Debug.Log("We just hit " +  hit.transform.name);

                // if distance between camera position and the hit point is greater than 2, improve accuracy of bullet, otherwise hit closest object
                if (Vector3.Distance(myCameraHead.position, hit.point) > 2f)
                {     
                    firePosition.LookAt(hit.point);

                    if(hit.collider.tag == "Shootable") 
                        Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));

                    if(hit.collider.tag == "Floor")
                        Instantiate(impactDebris, hit.point, Quaternion.LookRotation(hit.normal));
                }

                if(hit.collider.tag == "Enemy") // allow program to handle faster moving bullets to destroy objects
                    Destroy(hit.collider.gameObject);
            }
            else
            {
                firePosition.LookAt(myCameraHead.position + (myCameraHead.forward * 50f));
            }

            Instantiate(muzzleFlash, firePosition.position, firePosition.rotation, firePosition);
            Instantiate(bullet, firePosition.position, firePosition.rotation);

        }
    }

    private void CameraMovement()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Change Axis
        cameraVerticalRotation -= mouseY;
        // Restrict camera rotation
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);

        // Set Axis at CameraHead
        myCameraHead.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);
    }

    void PlayerMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 movement = x * transform.right + z * transform.forward;
        
        // Manage sprint speed
        if(Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            movement = movement * runSpeed * Time.deltaTime;

            isRunning = true;
        }
        // manage crouch speed
        else if (isCrouching)
        {
            movement = movement * crouchSpeed * Time.deltaTime;
        }
        else
        {
            movement = movement * speed * Time.deltaTime;

            isRunning = false;
        }

        myAnimator.SetFloat("PlayerSpeed", movement.magnitude);
        Debug.Log(movement.magnitude);

        myController.Move(movement);

        // Create gravity for the player, Increase velocity
        velocity.y += Physics.gravity.y * Mathf.Pow(Time.deltaTime, 2) * gravityModifier;
        // Check if controller is on the ground, if so, reset velocity 
        if(myController.isGrounded)
            velocity.y = Physics.gravity.y * Time.deltaTime;

        // Add Gravity to the player
        myController.Move(velocity);



    }

    private void SlideCounter()
    {
        if (startSliderTimer) 
        {
            currentSlideTimer = Time.deltaTime;
        }
        
    }
}
