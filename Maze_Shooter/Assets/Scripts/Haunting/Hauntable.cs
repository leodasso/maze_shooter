using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Arachnid;

namespace ShootyGhost
{
    [TypeInfoBox("Can be haunted by Haunter!")]
    public class Hauntable : MonoBehaviour
    {
		static List<Collection> haunted = new List<Collection>();

        public UnityEvent onHaunted;
        public UnityEvent onUnHaunted;

		[SerializeField, ToggleLeft, Tooltip("Have a special transition duration for exiting this haunter?")]
		bool customTransitionTime;

		[SerializeField, ShowIf("customTransitionTime")]
		float transitionTime;

		[ToggleLeft, Tooltip("Allow the player to exit this hauntable whenever the want")]
		public bool allowManualExit = true;

		[ToggleLeft, Tooltip("Enable if you want to specify a transform position for the ghost to return to when the haunt is complete")]
		public bool customReturnPos;
		[ShowIf("customReturnPos")]
		public Transform returnPos;

		public Collection hauntableType;

		[AssetsOnly]
		public GameObject hauntConstellationPrefab;

		[ReadOnly]
		public Haunter haunter;

		Health health;

		int initLayer;

		void Start() 
		{
			initLayer = gameObject.layer;
			health = GetComponent<Health>();
		}

        /// <summary>
        /// Calculates and returns the position that the ghost should go to once exiting after haunt
        /// </summary>
        public Vector3 GetReturnPosition()
        {
			if (customReturnPos)
				return returnPos.position;

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

		bool BeenHauntedThisSession() {
			return haunted.Contains(hauntableType);
		}

		public void AttemptHaunt(Haunter newHaunter) 
		{
			if (!enabled) return;
			
			// instantiate the haunt constellation
			HauntConstellation hauntConstellation = 
				Instantiate(hauntConstellationPrefab, transform.position, Quaternion.identity)
				.GetComponent<HauntConstellation>();

			hauntConstellation.hauntable = this;

			// show a shorter sequence if this isn't the first time haunting this thing
			if (BeenHauntedThisSession()) 
				hauntConstellation.PlayQuickSequence();
			else 
				hauntConstellation.PlayFullSequence();
		}

		/// The haunt will be accepted or rejected based on if the haunter
		/// has enough stars to haunt this object
		public void AcceptHaunt() 
		{
			if (!haunted.Contains(hauntableType) && hauntableType != null)
				haunted.Add(hauntableType);

			onHaunted.Invoke();
			LinkHealth();

			gameObject.layer = LayerMask.NameToLayer("Player");
		}

		void LinkHealth() 
		{
			if (!health || !haunter) {
				Debug.Log("Components are missing for health link!", gameObject);
				return;
			}

			health.mainHealth = haunter.GetComponent<Health>();
		}

		void UnlinkHealth() 
		{
			if (health) health.mainHealth = null;
		}

		/// <summary>
		/// Haunt was rejected before it could be accepted.
		/// </summary>
		public void RejectHaunt() 
		{
			EndHaunt();
		}

		/// <summary>
		/// Handles all un-haunting for this particular object.
		/// </summary>
        public void OnUnHaunted()
        {
            onUnHaunted.Invoke();
			UnlinkHealth();
			haunter = null;
        }

		/// <summary>
		/// Forces haunter to un-haunt this instance.
		/// </summary>
		public void EndHaunt() {
			if (!haunter) return;

			if (customTransitionTime)
				haunter.EndHaunt(transitionDuration: transitionTime);
			else
				haunter.EndHaunt();

			gameObject.layer = initLayer;
		}

		public void EndHauntWithNoTransition() {
			if (!haunter) return;
			haunter.EndHaunt(null, false);
		}

		void OnDisable()
		{
			EndHaunt();
		}
    }
}