using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 12.5f;

    public CharacterController myController;
    public Transform myCameraHead;

    public float mouseSensitivity = 100f;
    private float cameraVerticalRotation;

    public GameObject bullet;
    public Transform firePosition;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        CameraMovement();
        Shoot();
    }

    private void Shoot()
    {
        if(Input.GetMouseButtonDown(0))
        {
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
        movement = movement * speed * Time.deltaTime;

        myController.Move(movement);
    }
}
