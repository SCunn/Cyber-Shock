using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Variable to control force of bullet
    public float speed;

    // Reference to Rigidbody component
    public Rigidbody myRigidBody;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BulletFly();
    }

    private void BulletFly()
    {
        myRigidBody.velocity = transform.forward * speed;
    }
}
