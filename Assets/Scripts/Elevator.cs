using Assets.Scripts;
using System.Collections;
using UnityEngine;

public class Elevator : Activatable
{

    public Vector3 StartPos;

    public Vector3 EndPos;

    public float TravelTime = 1;

    public float MovementWait = 1;

    private float travelSpeed;

    private Vector3 nextPos;

    private bool movement = true;

    protected override bool Active { get; set; }

    protected override bool _needsActivating { get; set; }

    public bool NeedsActivating;

    // Start is called before the first frame update
    void Start()
    {
        _needsActivating = NeedsActivating;

        Active = !_needsActivating;

        transform.position = StartPos;
        nextPos = EndPos;

        var distance = (StartPos - EndPos).magnitude;
        travelSpeed = distance / (50 / (1 / TravelTime));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movement && Active)
        {
            StartCoroutine(verticalMovement(MovementWait));
        }
    }

    IEnumerator verticalMovement(float seconds)
    {
        if ((transform.position - nextPos).magnitude == 0)
        {
            movement = false;
            yield return new WaitForSeconds(seconds);
            movement = true;
            nextPos = nextPos == StartPos ? EndPos : StartPos;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, travelSpeed);
    }

    public override void Activate()
    {
        Active = true;
    }

    public override void Deactivate()
    {
        Active = false;
    }

}
