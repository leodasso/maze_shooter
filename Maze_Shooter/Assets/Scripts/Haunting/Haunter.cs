using System.Collections;
using UnityEngine;
using Rewired;
using Arachnid;
using Sirenix.OdinInspector;

namespace ShootyGhost
{    
    public class Haunter : MonoBehaviour
    {
        Rewired.Player _player;

		[Tooltip("The max distance you can haunt")]
        public FloatReference hauntDistance;

		[Tooltip("The time it takes to move the haunt trigger through the full animation")]
        public FloatReference hauntAnimDuration;

		[Tooltip("Offset of time between haunt transition hitting the ground and playing the 'reforming' animation")]
		public FloatReference hauntTransitionOffset;

		[Space]
		[SerializeField, Tooltip("How long in seconds it takes for one candle to burn.")]
		FloatReference candleBurnDuration;

		[SerializeField, Tooltip("Ref to the player's candle bag (how many candles they have)")]
		FloatValue candleBag;

		[SerializeField, Tooltip("Ref to the number of candles currently burning (in use for haunting)")]
		IntValue burningCandles;

		[Space]
		[SerializeField, Tooltip("Layers that I will collide with when returning from a haunted object")]
		LayerMask hauntReturnColliders;

        [Range(0, 1), Space]
        [Tooltip("When 'exiting' a haunted thing, the haunted object blows up like a balloon, about to pop. The amount it's" +
                 " blown up is the haunt burst intensity. When it reaches 1, this will exit the haunted and return to normal.")]
        public float hauntBurstIntensity = 0;

        [Tooltip("Updating a float value with the value of haunt burst allows other things like camera" +
                                   "and particles to change based on haunt burst value")] 
        public FloatValue hauntBurstIntensityRef;


		[Title("Prefabs"), Space]
        public GameObject transitionEffect;
        public GameObject hauntIndicatorPrefab;
		public GameObject hauntDashPrefab;

		[Title("Scene Refs"), Space]
		[SerializeField, Tooltip("Ref to my capsule collider. Used when casting to return from haunt")]
		new CapsuleCollider collider;
		/// <summary> The actual hauntable currently being controlled/haunted </summary>
        [Tooltip("The actual hauntable currently being controlled/haunted")]
        [ReadOnly]
        public Hauntable haunted;
		public GameObject hauntTrigger;

		[Space]
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
        
        void Start()
        {
            _player = ReInput.players.GetPlayer(0);
			ResetHauntTrigger();

			burningCandles.Value = 0;
        }

		void RefreshInput() 
		{
			_moveInput = new Vector2(_player.GetAxis("moveX"), _player.GetAxis("moveY"));
			_fireInput = new Vector2(_player.GetAxis("fireX"), _player.GetAxis("fireY"));
		}


        void Update()
        {
            if (_player == null) return;
			RefreshInput();
            if (_player.GetButton("haunt") || _fireInput.magnitude > .25f)
				playMaker.SendEvent("beginHauntTargeting");

			if (haunted && Time.timeScale > Mathf.Epsilon) 
			{
				float actualBurnDuration = candleBurnDuration.Value / haunted.hauntCost;
				float burnRate = 1 / actualBurnDuration;
				float burnThisFrame = burnRate * Time.deltaTime;
				candleBag.Value -= burnThisFrame;

				if (candleBag.Value < haunted.hauntCost) {
					candleBag.Value = haunted.hauntCost;
					EndHaunt();
				}
			}
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

		// Intended to be called from the haunted when the haunt is actually fully accepted
		public void SetBurningCandlesValue(int cost)
		{
			burningCandles.Value = cost;
		}

        /// <summary>
        /// Returns the ghost from posessing whatever it is currently haunting to its true form.
        /// </summary>
        public void EndHaunt(GameObject overrideHauntedObject = null, bool useTransition = true, float transitionDuration = .55f)
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

                if (useTransition && GhostTools.SafeToInstantiate(gameObject)) {
					GameObject hauntedObject = overrideHauntedObject ? overrideHauntedObject : haunted.gameObject;
					SpawnTransitionObject( transform.position, hauntedObject, transitionDuration);
				}
                haunted.OnUnHaunted();
            }
            
			playMaker.FsmVariables.GetFsmFloat("transitionDuration").Value = transitionDuration + hauntTransitionOffset.Value;
			playMaker.SendEvent("endHaunt");
            haunted = null;

			burningCandles.Value = 0;
        }

		/// <summary>
		/// Doesn't do all unhaunt behaviors, just sends events to haunted (if any)
		/// </summary>
		public void EndHauntForHaunted() 
		{
			if (haunted) haunted.OnUnHaunted();
		}
        
        ArcMover SpawnTransitionObject( Vector3 returnPos, GameObject haunted, float duration = .45f)
        {
            GameObject transition = Instantiate(transitionEffect, transform.position, transform.rotation);

            ArcMover arcMover = transition.GetComponent<ArcMover>();            
            arcMover.DoTransition(haunted, returnPos, duration);

            return arcMover;
        }
        
        void OnDestroy()
        {
            ClearHauntBurstEffect();
			if (haunted)
				haunted.OnHaunterDestroyed();
        }
    }
}