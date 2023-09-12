using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // Variable to control force of bullet
    public float speed, bulletLife;

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


        bulletLife -= Time.deltaTime; // bulletLife = bulletLife - Time.deltaTime

        

        if (bulletLife < 0)
        {
            Destroy(gameObject);
        }
    }

    private void BulletFly()
    {
        myRigidBody.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Enemy")
        //    Destroy(other.gameObject);

        Destroy(gameObject);
    }
}
