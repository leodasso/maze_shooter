using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace ShootyGhost
{
    [TypeInfoBox("Creates events for overlapping with hauntable things")]
    public class HauntTrigger : MonoBehaviour
    {
        public UnityEvent onHauntableOverlapped;
        public UnityEvent onHauntableExited;

        [Tooltip("The Haunter that this trigger is related to. It's required that this is the child of " +
                 "the Haunter.")]
        public Haunter haunter;

        GameObject _indicator;

        void Start()
        {
            haunter = GetComponentInParent<Haunter>();
        }

        void OnTriggerEnter(Collider other)
        {
            Hauntable hauntable = other.GetComponent<Hauntable>();
            if (!hauntable) return;
            
            hauntable.TargetedForHaunt();
            haunter.SetTargetedHauntable(hauntable);
            onHauntableOverlapped.Invoke();
        }

        void OnTriggerExit(Collider other)
        {
            Hauntable hauntable = other.GetComponent<Hauntable>();
            if (!hauntable) return;
            
            hauntable.UnTargetedForHaunt();

            if (hauntable == haunter.targetedHauntable)
            {
                haunter.ClearTargetedHauntable();
                onHauntableExited.Invoke();
            }
        }
    }
}
