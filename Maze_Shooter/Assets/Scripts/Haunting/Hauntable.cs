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

		public GameObject hauntConstellationPrefab;

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
				if (player.moveInput.magnitude > .5f)
					dir = (Vector3)player.moveInput;
			}

			Vector3 dirtyPos = transform.position + dir * 5;

            // TODO lol this prob needs better options
            return new Vector3(dirtyPos.x, 0, dirtyPos.z);
        }

		public void AttemptHaunt(Haunter newHaunter) 
		{
			// instantiate the haunt constellation
			HauntConstellation hauntConstellation = 
				Instantiate(hauntConstellationPrefab, transform.position, Quaternion.identity)
				.GetComponent<HauntConstellation>();

			hauntConstellation.hauntable = this;
			hauntConstellation.PlayFullSequence();
		}

		/// The haunt will be accepted or rejected based on if the haunter
		/// has enough stars to haunt this object
		public void AcceptHaunt() 
		{
			onHaunted.Invoke();
		}

		public void RejectHaunt() 
		{
			EndHaunt();
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