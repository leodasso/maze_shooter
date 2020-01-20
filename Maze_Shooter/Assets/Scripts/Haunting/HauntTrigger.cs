using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine.Events;

namespace ShootyGhost
{
    [TypeInfoBox("Creates events for overlapping with hauntable things")]
    public class HauntTrigger : MonoBehaviour
    {
        [Tooltip("Sits above the hauntable object to indicate that it's selected and is hauntable")]
        public GameObject indicatorPrefab;
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

            Vector3 otherPos = other.transform.position;
            Vector3 spawnPos = new Vector3(otherPos.x, otherPos.y + other.bounds.size.y, otherPos.z);
            _indicator = Instantiate(indicatorPrefab, spawnPos, quaternion.identity, other.transform);

            haunter.targetedHauntable = hauntable;
            onHauntableOverlapped.Invoke();
        }

        void OnTriggerExit(Collider other)
        {
            Hauntable hauntable = other.GetComponent<Hauntable>();
            if (!hauntable) return;
            
            hauntable.UnTargetedForHaunt();

            if (hauntable == haunter.targetedHauntable)
            {
                if (_indicator) Destroy(_indicator);
                haunter.targetedHauntable = null;
                onHauntableExited.Invoke();
            }
        }
    }
}
