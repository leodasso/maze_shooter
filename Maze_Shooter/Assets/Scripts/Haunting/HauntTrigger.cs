using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace ShootyGhost
{
    [TypeInfoBox("Creates events for overlapping with hauntable things")]
    public class HauntTrigger : MonoBehaviour
    {
        public UnityEvent onHauntableOverlapped;

        [Tooltip("The Haunter this trigger is related to. It's required that this is the child of the Haunter.")]
        public Haunter haunter;

        void Start()
        {
            haunter = GetComponentInParent<Haunter>();
        }

        void OnTriggerEnter(Collider other)
        {
            Hauntable hauntable = other.GetComponent<Hauntable>();
            if (!hauntable) return;
			if (!hauntable.enabled) return;
			haunter.BeginHaunt(hauntable);
        }

        
        void OnTriggerExit(Collider other)
        {
            Hauntable hauntable = other.GetComponent<Hauntable>();
            if (!hauntable) return;
        }
    }
}
