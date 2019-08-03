using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public Vector3 startPos;

    public Vector3 endPos;

    public float travelTime = 1;

    private float travelSpeed;

    private Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = startPos;
        nextPos = endPos;

        var distance = (startPos - endPos).magnitude;
        travelSpeed = distance / (50 / (1/(float)travelTime));
    }

    // FixedUpdate is called 50 times per second
    void FixedUpdate()
    {
        if ((transform.position - nextPos).magnitude <= 0)
        {
            nextPos = nextPos == startPos ? endPos : startPos;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPos, travelSpeed);
    }
}
