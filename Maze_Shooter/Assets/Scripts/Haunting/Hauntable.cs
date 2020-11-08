using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace ShootyGhost
{
    [TypeInfoBox("Can be haunted by Haunter!")]
    public class Hauntable : MonoBehaviour
    {
        public UnityEvent onHaunted;
        public UnityEvent onUnHaunted;
        public GameObject hauntedEffectPrefab;
        
        GameObject _hauntedEffectInstance;

        /// <summary>
        /// Calculates and returns the position that the ghost should go to once exiting after haunt
        /// </summary>
        public Vector3 GetReturnPosition()
        {
            // TODO lol this prob needs better options
            return transform.position + Vector3.left * 5;
        }

        [Button]
        public void OnIsHaunted(Haunter newHaunter)
        {
            InstantiateHauntEffect();
            onHaunted.Invoke();
        }

        public void OnUnHaunted()
        {
			Debug.Log("On Unhaunted was invoked");
            if (_hauntedEffectInstance)
                Destroy(_hauntedEffectInstance);
            
            onUnHaunted.Invoke();
        }

        void InstantiateHauntEffect()
        {
            if (hauntedEffectPrefab)
            {
                _hauntedEffectInstance =
                    Instantiate(hauntedEffectPrefab, transform.position, Quaternion.identity, transform);
            }
        }
    }
}