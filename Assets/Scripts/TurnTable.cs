using Assets.Scripts;
using UnityEngine;

public class TurnTable : MonoBehaviour
{

    public float rotateTime;

    public RotateDirection rotateDirection;

    // Start is called before the first frame update
    public virtual void Start()
    {
        rotateTime = rotateTime == 0 ? 1 : rotateTime;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        float rotateDelta = 360 / (50 / (1 / rotateTime));

        if (rotateDirection == RotateDirection.AntiClockwise)
        {
            rotateDelta *= -1;
        }
        transform.Rotate(0, rotateDelta, 0);

    }
}
