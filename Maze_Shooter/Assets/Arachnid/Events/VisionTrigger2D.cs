using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Arachnid
{
    [TypeInfoBox("Uses the attached 2D collider to trigger events, but uses raycasting to simulate vision")]
    public class VisionTrigger2D : FilteredTrigger2D
    {
        [UnityEngine.Tooltip("The transform representing the eyes. This will be used to determine if the triggerer can be seen.")]
        public Transform eyes;
        public UnityEventGameObjectParam onTriggeredByObject;

        //public TargetHolder targetHolder;

        protected override void OnTriggered(Collider2D triggerer)
        {
            // TODO raycast
            base.OnTriggered(triggerer);

            Debug.Log("invoking ontriggered with ", triggerer.gameObject);
            onTriggeredByObject.Invoke(triggerer.gameObject);
        }
    }
}