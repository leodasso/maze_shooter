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

		[SerializeField, ToggleLeft, Tooltip("Have a special transition duration for exiting this haunter?")]
		bool customTransitionTime;

		[SerializeField, ToggleLeft, Tooltip("Damage the haunter (if any) rather than this object")]
		bool linkHealth = true;

		[SerializeField, ShowIf("customTransitionTime")]
		float transitionTime;

		[ToggleLeft, Tooltip("Allow the player to exit this hauntable whenever the want")]
		public bool allowManualExit = true;

		[ToggleLeft, Tooltip("Enable if you want to specify a transform position for the ghost to return to when the haunt is complete")]
		public bool customReturnPos;
		[ShowIf("customReturnPos")]
		public Transform returnPos;

		public Collection hauntableType;

		[MinValue(0)]
		[ValidateInput("HauntCostValid", "If there is a cost to haunting, manual exit should always be allowed")]
		public int hauntCost = 1;

		GameObject hauntConstellationPrefab => GameMaster.Get().hauntConstellationPrefab;

		[ReadOnly]
		public Haunter haunter;

		[Space]
		public UnityEvent onHaunted;
        public UnityEvent onUnHaunted;

		Health health;


		int initLayer;

		void Start() 
		{
			initLayer = gameObject.layer;
			health = GetComponent<Health>();
		}

		bool HauntCostValid() 
		{
			if (!allowManualExit) return hauntCost == 0;
			return true;
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

			hauntConstellation.Init(this, hauntCost);

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

			if (linkHealth)
				LinkHealth();

			haunter.SetBurningCandlesValue(hauntCost);

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
        }

		public void OnHaunterDestroyed()
		{
			if (linkHealth)
				health.HaunterSafeDestruct();

		}

		void Reset()
		{
			UnlinkHealth();
			haunter = null;
			gameObject.layer = initLayer;
		}

		/// <summary>
		/// Forces haunter to un-haunt this instance.
		/// </summary>
		public void EndHaunt() 
		{
			if (!haunter) return;
			
			if (customTransitionTime)
				haunter.EndHaunt(transitionDuration: transitionTime);
			else
				haunter.EndHaunt();

			Reset();
		}

		public void EndHauntWithNoTransition() 
		{
			if (!haunter) return;
			haunter.EndHaunt(null, false);
			Reset();
		}

		void OnDisable()
		{
			EndHaunt();
		}
    }
}