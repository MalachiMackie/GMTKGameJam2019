using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody rigidBody;
    public float maxSpeed;
    public float acceleration;
    public Vector3 StartPos;


    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            transform.position = StartPos;
        }
    }

    void FixedUpdate()
    {
        Debug.Log(rigidBody.velocity.magnitude);
        if (Input.GetKey(KeyCode.W))
        {
            if (rigidBody.velocity.magnitude <= maxSpeed )
            {
                rigidBody.AddRelativeForce(0, 0, acceleration);
            }
        }
        
    }
}
