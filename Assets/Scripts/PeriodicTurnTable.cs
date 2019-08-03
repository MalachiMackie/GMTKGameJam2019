using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    class PeriodicTurnTable : MonoBehaviour
    {
        public GameObject RotatingObject;

        private bool rotating = true;

        public float PauseTime = 1;

        public int Intervals = 4;

        public float RotateTime = 4;

        public RotateDirection RotateDirection;

        public void Start()
        {
            
        }

        public void FixedUpdate()
        {
            if (rotating)
            {
                StartCoroutine(Rotate(PauseTime));
            }
        }

        private IEnumerator Rotate(float seconds)
        {
            float rotateDelta = 360 / (50 / (1 / RotateTime));

            if (RotateDirection == RotateDirection.AntiClockwise)
            {
                rotateDelta *= -1;
            }

            transform.Rotate(0, rotateDelta, 0);

            var yRotation = transform.localRotation.eulerAngles.y;

            var a = yRotation % (360/4);

            if (a < rotateDelta)
            {
                rotating = false;
                yield return new WaitForSeconds(seconds);
                transform.eulerAngles = new Vector3(0, yRotation + 0.5f, 0);
                rotating = true;
            }
        }

    }
}
