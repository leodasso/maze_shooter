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

		[ReadOnly]
		public Haunter haunter;
        
        GameObject _hauntedEffectInstance;

        /// <summary>
        /// Calculates and returns the position that the ghost should go to once exiting after haunt
        /// </summary>
        public Vector3 GetReturnPosition()
        {
			Vector3 dir = Vector3.left;
			Player player = GetComponent<Player>();
			if (player) {
				dir = (Vector3)player.moveInput;
			}

			
			
            // TODO lol this prob needs better options
            return transform.position + dir * 5;
        }

        [Button]
        public void OnIsHaunted(Haunter newHaunter)
        {
            onHaunted.Invoke();
        }

        public void OnUnHaunted()
        {
			Debug.Log("On Unhaunted was invoked");
            if (_hauntedEffectInstance)
                Destroy(_hauntedEffectInstance);
            
            onUnHaunted.Invoke();
			haunter = null;
        }
    }
}