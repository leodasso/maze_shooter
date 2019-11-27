using System.Collections;
using Arachnid;
using UnityEngine;
using Rewired;
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
    
    public class Haunter : MonoBehaviour
    {
        Rewired.Player _player;

        [TabGroup("main"), Tooltip("Haunt juice is needed to perform hauntings!")]
        public float hauntJuice = 0;

        [TabGroup("main"), Tooltip("The cost of performing a haunt")]
        public FloatReference hauntCost;

        [TabGroup("main"), Tooltip("The cooldown for returning to targeting mode once it's exited")]
        public FloatReference targetingModeCooldown;

        [TabGroup("main"), ReadOnly]
        public GhostState ghostState = GhostState.Normal;

        [Tooltip("If there is a hauntable selected, it will be haunted when haunt state is exited.")]
        [TabGroup("main"), ReadOnly]
        public Hauntable targetedHauntable;

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

        [TabGroup("events"), LabelText("onTargetingBegin")]
        public UnityEvent onHauntStateBegin;
        [TabGroup("events"), LabelText("onTargetingEnd")]
        public UnityEvent onHauntStateEnd;
        
        [TabGroup("events"), Space]
        public UnityEvent onHauntBegin;

        [TabGroup("events")] 
        public UnityEvent onHauntEnd;

        Rigidbody2D _rigidbody2D;
        float _targetingModeTimer = 0;
        
        // Start is called before the first frame update
        void Start()
        {
            _player = ReInput.players.GetPlayer(0);
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_player == null) return;

            // Cooldown for targeting mode entry. Prevents player from spamming targeting mode quickly
            if (_targetingModeTimer >= 0)
                _targetingModeTimer -= Time.unscaledDeltaTime;

            // Input for entering haunt targeting mode
            if (_player.GetButtonDown("haunt"))
            {
                if (CanBeginHauntTargeting)
                    BeginHauntTargeting();
            }
                
            // Input for leaving haunt targeting mode
            if (_player.GetButtonUp("haunt"))
            {
                if (CanHaunt)
                    EndHauntTargeting();
            }

            // Input for leaving a haunted object. The player must hold down 
            // the haunt button until intensity reaches 1 to exit.
            if (ghostState == GhostState.Haunting && _player.GetButton("haunt"))
            {
                hauntBurstIntensity += Time.unscaledDeltaTime;
                if (hauntBurstIntensity >= 1)
                    EndHaunt();
            }
            else hauntBurstIntensity = 0;
            hauntBurstIntensityRef.Value = hauntBurstIntensity;
        }

        bool CanBeginHauntTargeting => ghostState == GhostState.Normal && _targetingModeTimer <= 0 && HasHauntJuice;
        bool CanHaunt => ghostState == GhostState.Targeting && HasHauntJuice;
        bool HasHauntJuice => hauntJuice >= hauntCost.Value;

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
            if (targetedHauntable)
                BeginHaunt(targetedHauntable);
            else 
                ghostState = GhostState.Normal;
            
            targetedHauntable = null;
            onHauntStateEnd.Invoke();

            _targetingModeTimer = targetingModeCooldown.Value;
        }

        void BeginHaunt(Hauntable newHaunted)
        {
            haunted = newHaunted;
            haunted.Posess();
            ghostState = GhostState.Haunting;
            onHauntBegin.Invoke();
            _rigidbody2D.simulated = false;
            StartCoroutine(MoveToHaunted());
            
            // Instantiate the transition visuals
            GameObject transition = Instantiate(transitionEffect, transform.position, transform.rotation);
            ArcMover arcMover = transition.GetComponent<ArcMover>();
            arcMover.startPosition = transform.position;
            arcMover.end = haunted.gameObject;
        }

        void EndHaunt()
        {
            transform.parent = null;
            ghostState = GhostState.Normal;
            _rigidbody2D.simulated = true;
            haunted.OnUnHaunted();
            onHauntEnd.Invoke();
            _rigidbody2D.simulated = true;
            haunted = targetedHauntable = null;
        }
        
        /// <summary>
        /// Wait out the delay (hauntMovementDelay) and then move this object
        /// into the haunted transform as a child.
        /// </summary>
        IEnumerator MoveToHaunted()
        {
            yield return new WaitForSecondsRealtime(hauntMovementDelay);
            transform.parent = haunted.transform;
            transform.localPosition = Vector3.zero;
        }

        // Make sure we're not leaving anything hanging if this is destroyed during targeting
        void OnDestroy()
        {
            onHauntStateEnd.Invoke();
            hauntBurstIntensityRef.Value = 0;
        }

        public void AddJuice(float amount)
        {
            hauntJuice += amount;
        }
    }
}