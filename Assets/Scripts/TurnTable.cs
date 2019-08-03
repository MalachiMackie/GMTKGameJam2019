using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    class TurnTable : MonoBehaviour
    {

        private bool rotating = true;

        public float PauseTime = 1;

        public int Intervals = 4;

        public float RotateTime = 4;

        private bool activated = false;

        public RotateDirection RotateDirection;

        public void Start()
        {
            
        }

        public void FixedUpdate()
        {
            if (rotating && activated)
            {
                StartCoroutine(Rotate(PauseTime));
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                activated = true;
            }
        }

        private IEnumerator Rotate(float seconds)
        {
            float rotateDelta = 360 / (50 / (1 / RotateTime));
            //Bug when rotation is anitclockwise
            if (RotateDirection == RotateDirection.AntiClockwise)
            {
                rotateDelta *= -1;
            }

            transform.Rotate(0, rotateDelta, 0);

            var yRotation = transform.localRotation.eulerAngles.y;

            if (Intervals == 0)
            {
                yield return null;
            }
            var a = yRotation % (360/Intervals);

            if (a < Mathf.Abs(rotateDelta))
            {
                rotating = false;
                yield return new WaitForSeconds(seconds);
                transform.eulerAngles = new Vector3(0, yRotation + Mathf.Sign(rotateDelta * 0.5f), 0);
                rotating = true;
            }
        }

    }
}
