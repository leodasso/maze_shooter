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

        void OnTriggerEnter2D(Collider2D other)
        {
            Hauntable hauntable = other.GetComponent<Hauntable>();
            if (!hauntable) return;

            _indicator = Instantiate(indicatorPrefab, other.transform.position, quaternion.identity, other.transform);

            haunter.targetedHauntable = hauntable;
            onHauntableOverlapped.Invoke();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Hauntable hauntable = other.GetComponent<Hauntable>();
            if (!hauntable) return;

            if (hauntable == haunter.targetedHauntable)
            {
                if (_indicator) Destroy(_indicator);
                haunter.targetedHauntable = null;
                onHauntableExited.Invoke();
            }
        }
    }
}
