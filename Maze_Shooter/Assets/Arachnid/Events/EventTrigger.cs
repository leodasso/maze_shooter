using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Arachnid {

    [RequireComponent(typeof(Collider))]
    public class EventTrigger : FilteredTrigger
    {
        [AssetsOnly]
        public List<GameEvent> events = new List<GameEvent>();
        public UnityEvent unityEvent;
        public UnityEvent onTriggerExit;

        protected override void OnTriggered(Collider triggerer)
        {
            unityEvent.Invoke();
            foreach (var e in events) e.Raise();
        }

        protected override void OnTriggerExited(Collider triggerer)
        {
            onTriggerExit.Invoke();
        }
    }
}