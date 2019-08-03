using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    private Rigidbody rigidBody;
    public float maxSpeed;
    public float acceleration;
    public Vector3 StartPos;

    public Text TutorialText1;
    public Text TutorialText2;

    private int impulseTimer = 0;


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

        if (other.gameObject.tag == "TurnTable")
        {
            transform.SetParent(other.gameObject.transform);
        }

        if (other.gameObject.tag == "Press_W_or_Up")
        {
            TutorialText1.text = "Press W or Up Arrow to move forward";
        }

        if (other.gameObject.tag == "Press_Space")
        {
            TutorialText2.text = "Press Space to Dash";
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "TurnTable")
        {
            transform.SetParent(null);
        }

        if (collider.gameObject.tag == "Press_W_or_Up")
        {
            TutorialText1.text = "";
        }

        if (collider.gameObject.tag == "Press_Space")
        {
            TutorialText2.text = "";
        }

    }

    void FixedUpdate()
    {
        if (impulseTimer > 0)
        {
            impulseTimer--;
            if (impulseTimer == 0)
            {
                print("Dash loaded");
            }

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
            impulseTimer = 2 * 50;
            print("Dashed");
        
        }
    }
}
