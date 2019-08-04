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

        public float RotationOffset = 0;
        
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

            if (Intervals == 0)
            {
                yield break;
            }

            var rotationInterval = CalculateInterval(transform.eulerAngles.y);

            print(rotationInterval);
            if (RotateDirection == RotateDirection.Clockwise)
            {
                if ((rotationInterval + rotateDelta) > (360/Intervals))
                {
                    while (rotationInterval > Mathf.Sign(rotateDelta) * 0.1)
                    {
                        transform.Rotate(0, Mathf.Sign(rotateDelta) * 0.1f, 0);
                        rotationInterval = CalculateInterval(transform.eulerAngles.y);
                    }

                    rotating = false;
                    yield return new WaitForSeconds(seconds);
                    rotating = true;
                }
            }
            else
            {
                if ((rotationInterval + rotateDelta) < 0)
                {
                    while ((rotationInterval + rotateDelta) > 0)
                    {
                        transform.Rotate(0, Mathf.Sign(rotateDelta) * 0.1f, 0);
                        rotationInterval = CalculateInterval(transform.eulerAngles.y);
                    }

                    rotating = false;
                    yield return new WaitForSeconds(seconds);
                    rotating = true;
                }
            }
        }

        private float CalculateInterval(float yRotation)
        {
            var rotationTestValue = yRotation - RotationOffset;

            if (rotationTestValue < 1)
            {
                rotationTestValue += 360;
            }

            return rotationTestValue % (360 / Intervals);
        }

        public override void Activate()
        {
            Active = true;
            StartCoroutine(Enable());
        }

        public override void Deactivate()
        {
            Active = false;
        }

    }
}
