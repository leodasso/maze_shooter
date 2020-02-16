using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Arachnid {

    [RequireComponent(typeof(Collider))]
    public class EventTriggerGameObject : FilteredTrigger
    {
        [AssetsOnly]
        public List<GameEvent> events = new List<GameEvent>();
        public UnityEventGameObjectParam onTriggerEnter;
        public UnityEventGameObjectParam onTriggerExit;

        protected override void OnTriggered(Collider triggerer)
        {
            onTriggerEnter.Invoke(triggerer.gameObject);
            foreach (var e in events) e.Raise();
        }

        protected override void OnTriggerExited(Collider triggerer)
        {
            onTriggerExit.Invoke(triggerer.gameObject);
        }
    }
}