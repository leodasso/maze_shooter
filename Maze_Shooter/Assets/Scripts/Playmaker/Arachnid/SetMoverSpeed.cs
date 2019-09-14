using System;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{

    public class SetMoverSpeed : FsmStateAction
    {
        [RequiredField] [CheckForComponent(typeof(SimpleMovement))]
        public FsmOwnerDefault gameObject;

        public FsmFloat speed;

        [Tooltip("Repeat every frame. Useful if the variables are changing.")]
        public bool everyFrame;

        SimpleMovement _mover;

        public override void Reset()
        {
            gameObject = null;
            _mover = null;
        }

        public override void OnEnter()
        {
            // get the animator component
            var go = Fsm.GetOwnerDefaultTarget(gameObject);

            if (go == null)
            {
                Finish();
                return;
            }

            _mover = go.GetComponent<SimpleMovement>();

            if (_mover == null)
            {
                Finish();
                return;
            }

            _mover.speedMultiplier = speed.Value;
            if (!everyFrame) Finish();
        }

        public override void OnUpdate()
        {
            _mover.speedMultiplier = speed.Value;
        }
    }
}
