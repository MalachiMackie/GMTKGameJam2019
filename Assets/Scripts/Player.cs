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

    public float CubeSize = 0.2f;
    public int CubesInRow = 5;
    float CubesPivotWidthDistance;
    float CubesPivotHeightDistance;
    Vector3 CubesPivot;
    public float ExplosionForce = 50;
    public float ExplosionRadius = 4;
    public float ExplosionUpward = 0.4f;


    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position;
        StartRotation = transform.rotation;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
        _playerHud = GetComponentInChildren<PlayerHud>();

        //calculate pivot distance
        CubesPivotWidthDistance = CubeSize * CubesInRow / 2;
        CubesPivotHeightDistance = CubeSize * CubesInRow;
        CubesPivot = new Vector3(CubesPivotWidthDistance, CubesPivotHeightDistance, CubesPivotWidthDistance);
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
                    Explode();
                    gameManager.ReloadAfterSeconds(2);
                    break;
                }
            case "TurnTable":
                {
                    var turnTable = other.gameObject.GetComponent<TurnTable>();
                    turnTable.Activate();
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

    void OnTriggerStay(Collider collider)
    {
        if (Constants.FloorTags.Contains(collider.gameObject.tag))
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
                rigidBody.AddRelativeForce(0, 0, acceleration * rigidBody.mass);
            }
        }

        if (Input.GetKey(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        rigidBody.AddRelativeForce(new Vector3(0, 0, 10 * rigidBody.mass), ForceMode.Impulse);
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

    private void Explode()
    {
        //gets camera and removes the player as a parent
        var camera = GetComponentInChildren<Camera>();
        camera.transform.SetParent(null);
        
        for (int x = 0; x < CubesInRow; x++)
        {
            for (int y = 0; y < (CubesInRow * 2); y++)
            {
                for (int z = 0; z < CubesInRow; z++)
                {
                    createPiece(x, y, z);
                }
            }
        }

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, ExplosionRadius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(ExplosionForce + Random.Range(0, 50), new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Random.Range(-0.2f, 0.2f), transform.position.z + Random.Range(-0.2f, 0.2f)), ExplosionRadius, ExplosionUpward +Random.Range(0.1f, 0.1f));
            }
        }
        // make player disappear
        gameObject.SetActive(false);
    }

    void createPiece(int x, int y, int z)
    {  
        // create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        piece.transform.position = transform.position + new Vector3(CubeSize * x, CubeSize * y, CubeSize * z) - CubesPivot;
        piece.transform.localScale = new Vector3(CubeSize, CubeSize, CubeSize);

        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = CubeSize;
        piece.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
    }
}
