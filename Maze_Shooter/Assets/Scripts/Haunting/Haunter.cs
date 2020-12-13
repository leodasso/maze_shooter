using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Rewired;
using Arachnid;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace ShootyGhost
{
    public enum GhostState
    {
        Normal = 0, 
        Targeting = 1,     // searching for a target to haunt
        Haunting = 2,      // currently controlling a hauntable thing
    }

    public enum HauntTransition
    {
        In,    // Moving into the haunted thing
        Out,   // Coming out from the haunted thing
    }
    
    public class Haunter : MonoBehaviour
    {
        Rewired.Player _player;

		public int hauntStars = 1;

		[TabGroup("main")]
        public GameObject hauntTrigger;
        
        [TabGroup("main"), Tooltip("On Start(), haunt stars value is pulled from save file using this. On Destroy(), it's saved.")]
        public SavedInt savedHauntStars;

		[TabGroup("main"), Tooltip("The max distance you can haunt")]
        public FloatReference hauntDistance;

        [TabGroup("main"), Tooltip("The cooldown for returning to targeting mode once it's exited")]
        public FloatReference targetingModeCooldown;

		[TabGroup("main"), Tooltip("The time it takes to move the haunt trigger through the full animation")]
        public FloatReference hauntAnimDuration;

        [TabGroup("main"), ReadOnly]
        public GhostState ghostState = GhostState.Normal;

        /// <summary> The actual hauntable currently being controlled/haunted </summary>
        [Tooltip("The actual hauntable currently being controlled/haunted")]
        [TabGroup("main"), ReadOnly]
        public Hauntable haunted;
        
        [TabGroup("main"), Tooltip("Delay between a successful haunt and actually placing this object inside" +
                                   " the haunted transform.")]
        public float hauntMovementDelay = .25f;
        
        [Range(0, 1), Space, TabGroup("main")]
        [Tooltip("When 'exiting' a haunted thing, the haunted object blows up like a balloon, about to pop. The amount it's" +
                 " blown up is the haunt burst intensity. When it reaches 1, this will exit the haunted and return to normal.")]
        public float hauntBurstIntensity = 0;

        [TabGroup("main"), Tooltip("Updating a float value with the value of haunt burst allows other things like camera" +
                                   "and particles to change based on haunt burst value")] 
        public FloatValue hauntBurstIntensityRef;

        [TabGroup("main")]
        public GameObject transitionEffect;

        [TabGroup("main")] 
        public GameObject hauntIndicatorPrefab;

        [TabGroup("events"), LabelText("onTargetingBegin")]
        public UnityEvent onHauntStateBegin;
        
        [TabGroup("events"), LabelText("onTargetingEnd")]
        public UnityEvent onHauntStateEnd;
        
        [TabGroup("events"), Space]
        public UnityEvent onHauntBegin;

        [TabGroup("events")] 
        public UnityEvent onHauntEnd;

        Rigidbody _rigidbody;
        float _targetingModeTimer = 0;
        GameObject _indicator;

		Vector2 _moveInput;
		Vector2 _fireInput;

		Vector3 _hauntDirection = Vector3.right;

        bool CanBeginHauntTargeting => ghostState == GhostState.Normal && _targetingModeTimer <= 0;
        bool CanHaunt => ghostState == GhostState.Targeting;

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
            _rigidbody = GetComponent<Rigidbody>();
            // hauntStars = savedHauntStars.GetValue();
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

            // Cooldown for targeting mode entry. Prevents player from spamming targeting mode quickly
            if (_targetingModeTimer >= 0)
                _targetingModeTimer -= Time.unscaledDeltaTime;


            // Input for entering haunt targeting mode
            if (HauntRequested() && CanBeginHauntTargeting)
				BeginHauntTargeting();

            if (ghostState == GhostState.Haunting)
            {
                if (haunted)
                    transform.position = haunted.transform.position;
            }
            
            // Input for leaving a haunted object. The player must hold down 
            // the haunt button until intensity reaches 1 to exit.
            if (ghostState == GhostState.Haunting && _player.GetButton("haunt"))
            {
                hauntBurstIntensity += Time.unscaledDeltaTime * 2;
                if (hauntBurstIntensity >= 1)
                    EndHaunt();
            }
            else hauntBurstIntensity = 0;
            hauntBurstIntensityRef.Value = hauntBurstIntensity;
        }

		bool HauntRequested() 
		{
			return _player.GetButtonDown("haunt") || _fireInput.magnitude > .25f;
		}

		public void AnimateHauntTarget() 
		{
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

			// If no hauntable was caught in the trigger by the end of the animation,
			// just return to normal mode
			if (ghostState == GhostState.Targeting) EndHauntTargeting();
		}

		void ResetHauntTrigger() 
		{
			hauntTrigger.SetActive(false);
			hauntTrigger.transform.localPosition = Vector3.zero;
		}


        /// <summary>
        /// Quits all forms of haunting, whether its targeting or actively haunting a target
        /// </summary>
        void QuitHaunting()
        {
            if (ghostState == GhostState.Haunting)
                EndHaunt();
            else EndHauntTargeting();
        }


        /// <summary>
        /// Haunt targeting is the state where time slows down
        /// as the player searches for an object or enemy to haunt.
        /// </summary>
        void BeginHauntTargeting()
        {
            ghostState = GhostState.Targeting;
            onHauntStateBegin.Invoke();
        }

        
        void EndHauntTargeting()
        {
            if (_indicator) 
                Destroy(_indicator);
            
			ghostState = GhostState.Normal;
            
            onHauntStateEnd.Invoke();
            
            _targetingModeTimer = targetingModeCooldown.Value;
        }

        public void BeginHaunt(Hauntable newHaunted)
        {
            haunted = newHaunted;
            onHauntBegin.Invoke();
            _rigidbody.isKinematic = true;

			newHaunted.haunter = this;
			newHaunted.AttemptHaunt(this);
            ghostState = GhostState.Haunting;
        }


        /// <summary>
        /// Returns the ghost from posessing whatever it is currently haunting to its true form.
        /// </summary>
        public void EndHaunt(GameObject overrideHauntedObject = null)
        {
			GameObject container = overrideHauntedObject ? overrideHauntedObject : haunted.gameObject;

            if (haunted)
            {
                transform.position = haunted.GetReturnPosition();
                SpawnTransitionObject(HauntTransition.Out, transform.position, container);
                haunted.OnUnHaunted();
            }
            
            ghostState = GhostState.Normal;
            onHauntEnd.Invoke();
            haunted = null;
        }

		public void MigrateHaunt(Hauntable newHauntable) {
			BeginHaunt(newHauntable);
		}

        
        ArcMover SpawnTransitionObject(HauntTransition transitionType, Vector3 start, GameObject destination)
        {
            GameObject transition = Instantiate(transitionEffect, transform.position, transform.rotation);
            ArcMover arcMover = transition.GetComponent<ArcMover>();
            arcMover.startPosition = start;
            arcMover.end = destination;
            
            if (transitionType == HauntTransition.In)
                arcMover.transitionIn.Invoke();
            else 
                arcMover.transitionOut.Invoke();

            return arcMover;
        }
        

        // Make sure we're not leaving anything hanging if this is destroyed during targeting
        void OnDestroy()
        {
            // savedHauntStars.Save(hauntStars);
            onHauntStateEnd.Invoke();
            hauntBurstIntensityRef.Value = 0;
        }
    }
}