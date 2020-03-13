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
        public UnityEvent onHauntableExited;
        public float loseHauntTargetDelay = .5f;

        [Tooltip("The Haunter this trigger is related to. It's required that this is the child of the Haunter.")]
        public Haunter haunter;

        readonly List<Hauntable> _overlappedHauntables = new List<Hauntable>();
        Hauntable _target;
        float _loseHauntTargetTimer = 0;

        void Start()
        {
            haunter = GetComponentInParent<Haunter>();
        }


        void Update()
        {
            if (!_target) return;
            
            // The haunt target shouldn't be lost immediately after overlap ends.
            // This gives a small buffer so even if the target isn't overlapped anymore, it will stay targeted.
            if (!_overlappedHauntables.Contains(_target))
            {
                _loseHauntTargetTimer += Time.unscaledDeltaTime;
                if (_loseHauntTargetTimer >= loseHauntTargetDelay)
                    LoseHauntTarget();
            }
            else _loseHauntTargetTimer = 0;
        }
        

        void OnTriggerEnter(Collider other)
        {
            Hauntable hauntable = other.GetComponent<Hauntable>();
            if (!hauntable) return;
            
            // add the hauntable to the list of overlapped hauntables
            if (!_overlappedHauntables.Contains(hauntable))
            {
                onHauntableOverlapped.Invoke();
                _overlappedHauntables.Add(hauntable);
            }
            
            // If there's not already a target, set this as the new target
            if (!_target)
                SetHauntTarget(hauntable);
        }

        
        void OnTriggerExit(Collider other)
        {
            Hauntable hauntable = other.GetComponent<Hauntable>();
            if (!hauntable) return;
            _overlappedHauntables.Remove(hauntable);
        }

        
        void SetHauntTarget(Hauntable newTarget)
        {
            _target = newTarget;
            haunter.SetPendingHauntable(newTarget);
        }


        void LoseHauntTarget()
        {
            if (!_target) return;
            if (_target == haunter.PendingHauntable)
            {
                haunter.ClearTargetedHauntable();
                onHauntableExited.Invoke();
            }

            _target = null;
        }
    }
}
