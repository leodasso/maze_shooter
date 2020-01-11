using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Arachnid
{

    public enum UpdateMode { Update = 0, FixedUpdate = 1, LateUpdate = 2, Start = 3}

    [ExecuteInEditMode]
    [AddComponentMenu("Arachnid/Transform/Set Rotation")]
    public class SetRotation : MonoBehaviour
    {
        public UpdateMode updateMode;
        public Space rotationSpace;
        public Vector3 eulers;
        public bool x = true;
        public bool y = true;
        public bool z = true;

        Vector3 _combinedEulers;

        // Use this for initialization
        void Start ()
        {
            if (updateMode == UpdateMode.Start)
                DoRotate();
        }

        void Update()
        {
            if (updateMode != UpdateMode.Update) return;
            DoRotate();
        }


        void LateUpdate ()
        {
            if (updateMode != UpdateMode.LateUpdate) return;
            DoRotate();
        }

        void FixedUpdate()
        {
            if (updateMode != UpdateMode.FixedUpdate) return;
            DoRotate();
        }

        void DoRotate()
        {
            if (rotationSpace == Space.Self)
            {
                _combinedEulers = new Vector3(
                    x ? eulers.x : transform.localEulerAngles.x,
                    y ? eulers.y : transform.localEulerAngles.y,
                    z ? eulers.z : transform.localEulerAngles.z
                    );
                transform.localEulerAngles = _combinedEulers;
            }

            if (rotationSpace == Space.World)
            {
                _combinedEulers = new Vector3(
                    x ? eulers.x : transform.eulerAngles.x,
                    y ? eulers.y : transform.eulerAngles.y,
                    z ? eulers.z : transform.eulerAngles.z
                );
                transform.eulerAngles = _combinedEulers;
            }
        }
    }
}