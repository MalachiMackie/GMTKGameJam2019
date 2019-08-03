using Assets.Scripts;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody rigidBody;
    public float maxSpeed;
    public float acceleration;
    public Vector3 StartPos;
    public Quaternion StartRotation;

    private int impulseTimer = 0;


    private float moveTimer = 0.5f;
    private bool canMove = true;
    private bool moveCounting = false;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        StartRotation = transform.rotation;
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
            transform.rotation = StartRotation;
        }

        if (other.gameObject.tag == "TurnTable")
        {
            transform.SetParent(other.gameObject.transform);
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "TurnTable")
        {
            transform.SetParent(null);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (Constants.FloorTags.Contains(collision.gameObject.tag))
        {
            if (moveCounting)
            {
                moveCounting = false;
            }
            if (!canMove)
            {
                print("Can start moving again");
                canMove = true;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (Constants.FloorTags.Contains(collision.gameObject.tag))
        {
            StartCoroutine(StopMovingAfterSeconds(moveTimer));
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Constants.FloorTags.Contains(collision.gameObject.tag))
        {
            print(moveCounting);
            
        }
    }

    IEnumerator StopMovingAfterSeconds(float seconds)
    {
        moveCounting = true;
        yield return new WaitForSeconds(seconds);
        if (moveCounting)
        {
            canMove = false;
            moveCounting = false;
        }
    }

    void FixedUpdate()
    {
        if (!canMove)
        {
            //print("Cant Move");
            return;
        }

        if (impulseTimer > 0)
        {
            impulseTimer -= 1/50;
            DashLoaded();
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (rigidBody.velocity.magnitude <= maxSpeed)
            {
                rigidBody.AddRelativeForce(0, 0, acceleration);
            }
        }

        if (Input.GetKey(KeyCode.Space) && impulseTimer == 0)
        {
            rigidBody.AddRelativeForce(new Vector3(0, 0, 55), ForceMode.Impulse);
            impulseTimer = 2;
        
        }
    }

    private void DashLoaded()
    {
        //TODO: Give feedback that the dash is loaded
    }
}
