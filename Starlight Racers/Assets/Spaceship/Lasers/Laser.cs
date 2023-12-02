using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float timer;

    private Rigidbody rb;

    [SerializeField]
    private float speed;
    
    private Vector3 prevVelocity;

    public void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        timer = 0f; //initialize the timer;
    }

    public void FixedUpdate()
    {
        rb.AddForce(transform.forward * speed);
        
        timer += Time.deltaTime;
        
        prevVelocity = rb.velocity; //update our previous velocity

        //If object reaches 2.5 seconds, destroy object
        if(timer >= 2.5f)
        {
            Destroy(gameObject);
        }

    }
}
