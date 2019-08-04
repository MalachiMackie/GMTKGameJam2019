using Assets.Scripts;
using System.Collections;
using UnityEngine;

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

    private PlayerHud _playerHud;

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        StartRotation = transform.rotation;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        _playerHud = GetComponentInChildren<PlayerHud>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Respawn":
                {
                    gameManager.ReloadScene();
                    break;
                }
            case "TurnTable":
                {
                    transform.SetParent(other.gameObject.transform);
                    break;
                }
            case "Finish":
                {
                    gameManager.NextLevel();
                    break;
                }
            case "Key":
                {
                    var keyScript = other.gameObject.GetComponent<Key>();
                    keyScript.Trigger();
                    break;
                }
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Elevator")
        {
            var elevator = collision.gameObject.GetComponent<Elevator>();
            if (elevator.NeedsActivating)
            {
                elevator.Activate();
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
        _playerHud.StartDash();
        yield return new WaitForSeconds(2);
        canDash = true;
        _playerHud.DashLoaded();
    }

    public void DisableDash()
    {
        canDash = false;
        _playerHud?.DisableDash();
    }

    public void EnableDash()
    {
        canDash = true;
        _playerHud?.EnableDash();
    }
}
