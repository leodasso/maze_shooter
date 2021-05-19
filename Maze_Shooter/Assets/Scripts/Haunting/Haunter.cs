using System.Collections;
using UnityEngine;
using Rewired;
using Arachnid;
using Sirenix.OdinInspector;

namespace ShootyGhost
{
    public enum HauntTransition
    {
        In,    // Moving into the haunted thing
        Out,   // Coming out from the haunted thing
    }
    
    public class Haunter : MonoBehaviour
    {
        Rewired.Player _player;

		public int hauntStars = 1;

        public GameObject hauntTrigger;
        
        [Tooltip("On Start(), haunt stars value is pulled from save file using this. On Destroy(), it's saved.")]
        public SavedInt savedHauntStars;

		[Tooltip("The max distance you can haunt")]
        public FloatReference hauntDistance;

		[Tooltip("The time it takes to move the haunt trigger through the full animation")]
        public FloatReference hauntAnimDuration;

		[Space]
		[SerializeField, Tooltip("Layers that I will collide with when returning from a haunted object")]
		LayerMask hauntReturnColliders;

		[SerializeField, Tooltip("Ref to my capsule collider. Used when casting to return from haunt")]
		new CapsuleCollider collider;

        /// <summary> The actual hauntable currently being controlled/haunted </summary>
        [Tooltip("The actual hauntable currently being controlled/haunted")]
        [ReadOnly]
        public Hauntable haunted;
        
        [Range(0, 1), Space]
        [Tooltip("When 'exiting' a haunted thing, the haunted object blows up like a balloon, about to pop. The amount it's" +
                 " blown up is the haunt burst intensity. When it reaches 1, this will exit the haunted and return to normal.")]
        public float hauntBurstIntensity = 0;

        [Tooltip("Updating a float value with the value of haunt burst allows other things like camera" +
                                   "and particles to change based on haunt burst value")] 
        public FloatValue hauntBurstIntensityRef;

        public GameObject transitionEffect;

        public GameObject hauntIndicatorPrefab;
		public GameObject hauntDashPrefab;

		[Tooltip("This component calls events: beginHauntTargeting, endHauntTargeting, haunt, endHaunt")]
		public PlayMakerFSM playMaker;

        GameObject _indicator;
		Vector2 _moveInput;
		Vector2 _fireInput;
		Vector3 _hauntDirection = Vector3.right;

		/// <summary>
		/// Returns the direction that the haunty ghost will shoot towards if activated (based on player input)
		/// </summary>
		Vector3 HauntDirection() 
		{
			if (_fireInput.magnitude > .1f) 	 
				_hauntDirection =  Math.Project2Dto3D(_fireInput.normalized);
			else if (_moveInput.magnitude > .1f)
				 _hauntDirection = Math.Project2Dto3D(_moveInput.normalized);
			return _hauntDirection;
		}
        
        // Start is called before the first frame update
        void Start()
        {
            _player = ReInput.players.GetPlayer(0);
			ResetHauntTrigger();
        }

		void RefreshInput() 
		{
			_moveInput = new Vector2(_player.GetAxis("moveX"), _player.GetAxis("moveY"));
			_fireInput = new Vector2(_player.GetAxis("fireX"), _player.GetAxis("fireY"));
		}


        // Update is called once per frame
        void Update()
        {
            if (_player == null) return;
			RefreshInput();
            if (_player.GetButtonDown("haunt") || _fireInput.magnitude > .25f)
				playMaker.SendEvent("beginHauntTargeting");
        }

		public void ClearHauntBurstEffect()
		{
			hauntBurstIntensity = 0;
            hauntBurstIntensityRef.Value = hauntBurstIntensity;
		}


		// Input for leaving a haunted object. The player must hold down 
		// the haunt button until intensity reaches 1 to exit.
		public void CheckForUnHauntInput()
		{
            if (CanUnHaunt() && _player.GetButton("haunt"))
            {
                hauntBurstIntensity += Time.unscaledDeltaTime * 2;
                if (hauntBurstIntensity >= 1)
                    EndHaunt();

				hauntBurstIntensityRef.Value = hauntBurstIntensity;
            }
			else ClearHauntBurstEffect();
		}


		public void MatchHauntedPosition()
		{
			transform.position = haunted.transform.position;
		}

		bool CanUnHaunt() 
		{
			if (haunted && !haunted.allowManualExit) return false;
			return true;
		}

		public void AnimateHauntTarget() 
		{
			SpriteAnimationPlayer hauntDash = 
				Instantiate(hauntDashPrefab, transform.position, Quaternion.identity).GetComponent<SpriteAnimationPlayer>();
			hauntDash.direction.customDirection = HauntDirection();
			hauntDash.gameObject.SetActive(true);
			hauntDash.PlayClipFromBeginning( () => {hauntDash.gameObject.SetActive(false);} );
			Vector3 localTriggerPos = HauntDirection() * hauntDistance.Value;
			StartCoroutine(AnimateHauntTargetRoutine(localTriggerPos));
		}

		IEnumerator AnimateHauntTargetRoutine(Vector3 finalLocalPos) 
		{
			float progress = 0;
			hauntTrigger.SetActive(true);

			while (progress < 1) {
				progress += Time.unscaledDeltaTime / hauntAnimDuration.Value;
				hauntTrigger.transform.localPosition = Vector3.Lerp(Vector3.zero, finalLocalPos, progress);
				yield return null;
			}
			ResetHauntTrigger();
		}

		void ResetHauntTrigger() 
		{
			hauntTrigger.SetActive(false);
			hauntTrigger.transform.localPosition = Vector3.zero;
		}

		public void MigrateHaunt(Hauntable newHauntable) 
		{
			BeginHaunt(newHauntable);
		}

        public void BeginHaunt(Hauntable newHaunted)
        {
			ResetHauntTrigger();
            haunted = newHaunted;
            playMaker.SendEvent("haunt");

			newHaunted.haunter = this;
			newHaunted.AttemptHaunt(this);
        }

        /// <summary>
        /// Returns the ghost from posessing whatever it is currently haunting to its true form.
        /// </summary>
        public void EndHaunt(GameObject overrideHauntedObject = null, bool useTransition = true, float transitionDuration = .35f)
        {
            if (haunted)
            {
				// store return position so we can raycast to it and make sure we don't cross any boundaries
				Vector3 destination = haunted.GetReturnPosition();
				Vector3 returnPos = destination;
				Vector3 destinationVector = destination - transform.position;

				RaycastHit hit;
				if (Physics.CapsuleCast(
					transform.position, 
					transform.position + collider.height * Vector3.up, 
					collider.radius,
					destinationVector, out hit, destinationVector.magnitude, hauntReturnColliders)) {
					returnPos = hit.point - destinationVector.normalized * collider.radius * 1.1f;
				}

				transform.position = GhostTools.GroundPoint(returnPos);

                if (useTransition) {
					GameObject hauntedObject = overrideHauntedObject ? overrideHauntedObject : haunted.gameObject;
					SpawnTransitionObject(HauntTransition.Out, transform.position, hauntedObject, transitionDuration);
				}
                haunted.OnUnHaunted();
            }
            
			playMaker.FsmVariables.GetFsmFloat("transitionDuration").Value = transitionDuration + .05f;
			playMaker.SendEvent("endHaunt");
            haunted = null;
        }
        
        ArcMover SpawnTransitionObject(HauntTransition transitionType, Vector3 start, GameObject destination, float duration = .45f)
        {
            GameObject transition = Instantiate(transitionEffect, transform.position, transform.rotation);
            ArcMover arcMover = transition.GetComponent<ArcMover>();
            arcMover.startPosition = start;
            arcMover.end = destination;
			arcMover.SetTransitionDuration(duration);
            
            if (transitionType == HauntTransition.In)
                arcMover.transitionIn.Invoke();
            else 
                arcMover.transitionOut.Invoke();

            return arcMover;
        }
        
        void OnDestroy()
        {
            ClearHauntBurstEffect();
        }
    }
}