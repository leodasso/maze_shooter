using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Arachnid {

    [RequireComponent(typeof(Collider))]
	[AddComponentMenu("Triggers/Event Trigger")]
    public class EventTrigger : FilteredTrigger
    {
        [AssetsOnly]
        public List<GameEvent> events = new List<GameEvent>();
        public UnityEvent unityEvent;
        public UnityEvent onTriggerExit;

        protected override void OnTriggered(Collider triggerer)
        {
			if (debug) Debug.Log(name + " successful trigger raised.");
            unityEvent.Invoke();
            foreach (var e in events) e.Raise();
        }

        protected override void OnTriggerExited(Collider triggerer)
        {
			if (debug) Debug.Log(name + " successful trigger exit raised.");
            onTriggerExit.Invoke();
        }
    }
}