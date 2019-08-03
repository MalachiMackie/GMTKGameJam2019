using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    class TurnTable : Activatable
    {
        public float StartUpTime = 0.5f;

        private bool rotating = true;

        public float PauseTime = 1;

        public int Intervals = 4;

        public float RotateTime = 4;

        private bool canRotate = false;

        protected override bool Active { get; set; }

        protected override bool _needsActivating { get; set; }

        public bool NeedsActivating;

        public RotateDirection RotateDirection;

        public void Start()
        {
            _needsActivating = NeedsActivating;
            Active = !_needsActivating;
        }

        public void FixedUpdate()
        {
            if (rotating && canRotate && Active)
            {
                StartCoroutine(Rotate(PauseTime));
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                StartCoroutine(Enable());
            }
        }

        private IEnumerator Enable()
        {
            yield return new WaitForSeconds(StartUpTime);
            canRotate = true;
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

            if (Intervals == 0)
            {
                yield break;
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

        public override void Activate()
        {
            Active = true;
        }

        public override void Deactivate()
        {
            Active = false;
        }

    }
}
