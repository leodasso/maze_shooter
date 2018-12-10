using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace Arachnid {

    [RequireComponent(typeof(Collider2D))]
    public class EventTrigger2D : MonoBehaviour
    {
        [AssetsOnly]
        public List<GameEvent> events = new List<GameEvent>();

        public UnityEvent uEvent;

        [ToggleLeft, Tooltip("Will only trigger once.")]
        public bool oneOff;

        [ToggleLeft, Tooltip("Only allow triggers from objects of a particular collection")]
        public bool filterTriggers = true;

        [ShowIf("filterTriggers"), Tooltip("Any object in one of these collections can trigger this."), AssetsOnly]
        public List<Collection> triggerers = new List<Collection>();

        bool _triggered;


        void OnTriggerEnter2D(Collider2D other)
        {
            if (!filterTriggers)
            {
                Trigger();
                return;
            }

            foreach (var c in triggerers)
            {
                if (c.ContainsGameObject(other.gameObject))
                {
                    Trigger();
                    return;
                }
            }
        }

        void Trigger()
        {
            if (_triggered && oneOff) return;
            
            _triggered = true;
            uEvent.Invoke();
            foreach (var e in events) e.Raise();
        }
    }
}