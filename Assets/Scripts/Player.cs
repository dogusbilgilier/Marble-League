using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody rb;
    float speed;
    Vector3 speedVector;
    [SerializeField] float defaultSpeed,speedUp,speedMax;
    void Start()
    {
        rb = transform.GetChild(0).GetComponent<Rigidbody>();
        speed = defaultSpeed;
        rb.velocity = Vector3.down;
        OnSpeedChange();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(rb.velocity);

        rb.velocity = speedVector;
    }
    void OnSpeedChange()
    {
        
        speedVector = new Vector3(rb.velocity.x *speed, rb.velocity.y * speed, rb.velocity.z * speed);
    }
}
