using Assets.Scripts;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private Rigidbody rigidBody;
    public float maxSpeed;
    public float acceleration;
    public Vector3 StartPos;
    public Quaternion StartRotation;

    public GameManager gameManager;

    private float moveTimer = 0.5f;
    private bool canMove = true;
    private bool moveCounting = false;

    private bool canDash = true;

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

        if (other.gameObject.tag == "Finish")
        {
            gameManager.NextLevel();
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
            return;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (rigidBody.velocity.magnitude <= maxSpeed)
            {
                rigidBody.AddRelativeForce(0, 0, acceleration);
            }
        }

        if (Input.GetKey(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        rigidBody.AddRelativeForce(new Vector3(0, 0, 55), ForceMode.Impulse);
        canDash = false;
        yield return new WaitForSeconds(2);
        canDash = true;
        //Give feedback that the dash is loaded
    }
}
