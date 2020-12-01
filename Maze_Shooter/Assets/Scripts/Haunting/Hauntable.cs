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

			Vector3 dirtyPos = transform.position + dir * 5;

            // TODO lol this prob needs better options
            return new Vector3(dirtyPos.x, 0, dirtyPos.z);
        }

        [Button]
        public void OnIsHaunted(Haunter newHaunter)
        {
            onHaunted.Invoke();
        }

        public void OnUnHaunted()
        {
            onUnHaunted.Invoke();
			haunter = null;
        }

		public void EndHaunt() {
			if (!haunter) return;
			haunter.EndHaunt();
		}
    }
}