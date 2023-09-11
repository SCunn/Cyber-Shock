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

    public GameObject muzzleFlash, bulletHole;
    

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
            RaycastHit hit;

            if(Physics.Raycast(myCameraHead.position, myCameraHead.forward, out hit, 100f))
            {
                //Debug.Log("We just hit " +  hit.transform.name);

                // if distance between camera position and the hit point is greater than 2, improve accuracy of bullet, otherwise hit closest object
                if (Vector3.Distance(myCameraHead.position, hit.point) > 2f)
                {     
                    firePosition.LookAt(hit.point);
                }
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
        movement = movement * speed * Time.deltaTime;

        myController.Move(movement);
    }
}
