﻿using System.Collections;
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

            var yRotation = transform.localRotation.eulerAngles.y;
            var rotationTestValue = yRotation - RotationOffset;

            if (rotationTestValue < 1)
            {
                rotationTestValue = rotationTestValue + 360;
            }

            if (Intervals == 0)
            {
                yield break;
            }

            var rotationInterval = rotationTestValue % (360/Intervals);

            if (rotationInterval < Mathf.Abs(rotateDelta))
            {
                //This is fucked, fix it
                transform.Rotate();
                transform.eulerAngles = new Vector3(0, b, 0);
                print(transform.eulerAngles);
                print(rotationInterval);
                rotating = false;
                yield return new WaitForSeconds(seconds);
                rotating = true;
            }
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
