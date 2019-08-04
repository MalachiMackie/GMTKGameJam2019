using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : Activatable
{
    public float TravelTime = 1;

    private float travelSpeed;

    private Vector3 nextPos;

    public float timeOffset = 0;

    private bool Activated;

    public bool Debug;

    public List<Vector3> TravelPoints = new List<Vector3>();

    public bool NeedsActivating;

    protected override bool _needsActivating { get; set; } = false;

    protected override bool Active { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        _needsActivating = NeedsActivating;
        transform.position = TravelPoints[0];
        nextPos = TravelPoints[1];
        
        travelSpeed = CalculateSpeed(transform.position, nextPos, TravelTime);

        if (!_needsActivating)
        {
            StartCoroutine(Utils.DoAfterSeconds(() => { Activated = true; }, timeOffset));
        }
    }

    // FixedUpdate is called 50 times per second
    void FixedUpdate()
    {
        if (Activated)
        {
            if ((transform.position - nextPos).magnitude <= 0)
            {
                NextPoint();
            }

            transform.position = Vector3.MoveTowards(transform.position, nextPos, travelSpeed);
        }
    }

    private void NextPoint()
    {
        var currentPos = nextPos;
        var index = TravelPoints.IndexOf(nextPos) + 1;
        if (index == TravelPoints.Count)
        {
            index = 0;
        }

        nextPos = TravelPoints[index];

        travelSpeed = CalculateSpeed(currentPos, nextPos, TravelTime);
    }

    public float CalculateSpeed(Vector3 pos1, Vector3 pos2, float travelTime)
    {
        var distance = (pos1 - pos2).magnitude;
        return distance / (50 / (1 / travelTime));
    }

    public override void Activate()
    {
        TravelTime = 1;
    }

    public override void Deactivate()
    {
        throw new System.NotImplementedException();
    }
}
