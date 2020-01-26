using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Rewired;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Unity.Mathematics;

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
        public int hauntJuice = 0;
        
        [TabGroup("main"), Tooltip("On Start(), hauntJuice value is pulled from save file using this. On Destroy(), it's saved.")]
        public SavedInt savedHauntJuice;

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

        [TabGroup("main")]
        public GameObject hauntPacketPrefab;

        [TabGroup("main")] 
        public GameObject hauntIndicatorPrefab;

        [TabGroup("UI")]
        public GameObject hauntJuiceQtyGuiPrefab;

        [TabGroup("UI")] 
        public float showTime = 5;

        [TabGroup("events"), LabelText("onTargetingBegin")]
        public UnityEvent onHauntStateBegin;
        [TabGroup("events"), LabelText("onTargetingEnd")]
        public UnityEvent onHauntStateEnd;
        
        [TabGroup("events"), Space]
        public UnityEvent onHauntBegin;

        [TabGroup("events")]
        public UnityEvent onJuiceAdded;

        [TabGroup("events")] 
        public UnityEvent onHauntEnd;

        Rigidbody _rigidbody;
        float _targetingModeTimer = 0;
        HauntJuiceGui _juiceGuiInstance;
        float _hauntGuiTimer;
        bool _hauntGuiTimed;
        List<HauntPacket> _hauntPackets = new List<HauntPacket>();
        GameObject _indicator;
        Hauntable _pendingHauntable;

        public int DisplayedHauntJuice => hauntJuice - _hauntPackets.Count;
        
        
        // Start is called before the first frame update
        void Start()
        {
            _player = ReInput.players.GetPlayer(0);
            _rigidbody = GetComponent<Rigidbody>();
            hauntJuice = savedHauntJuice.GetValue();
        }

        // Update is called once per frame
        void Update()
        {
            if (_player == null) return;
            
            if (ghostState == GhostState.Targeting) 
                TargetingUpdate();
            
            // Haunt GUI shows up when picking up haunt juice. This timer removes it after a certain amt of time.
            if (_hauntGuiTimed && ghostState == GhostState.Normal)
            {
                if (_hauntGuiTimer > 0)
                    _hauntGuiTimer -= Time.unscaledDeltaTime;
                else
                    HideJuiceGui();
            }

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

        void TargetingUpdate()
        {
            if (!targetedHauntable) return;
            if (!targetedHauntable.CostIsFulfilled())
                SendHauntPacket();
        }

        public void SetTargetedHauntable(Hauntable newTarget)
        {
            targetedHauntable = newTarget;
        }

        public void ClearTargetedHauntable()
        {
            targetedHauntable = null;
            _pendingHauntable = null;
            if (_indicator) 
                Destroy(_indicator);
            RecallHauntPackets();
        }

        void SendHauntPacket()
        {
            if (!targetedHauntable) return;
            if (_hauntPackets.Count >= targetedHauntable.hauntCost) return;

            HauntPacket newHauntPacket = Instantiate(hauntPacketPrefab, transform.position, quaternion.identity)
                .GetComponent<HauntPacket>();
            
            newHauntPacket.Init(this, targetedHauntable);
            _hauntPackets.Add(newHauntPacket);
        }

        public void TakeBackHauntPacket(HauntPacket packet)
        {
            _hauntPackets.Remove(packet);
        }

        void RecallHauntPackets()
        {
            foreach (var packet in _hauntPackets)
                packet.ReturnToHaunter();
            
            _hauntPackets.Clear();
        }

        void DestroyHauntPackets()
        {
            foreach (var packet in _hauntPackets) 
                Destroy(packet);
            
            _hauntPackets.Clear();
        }

        /// <summary>
        /// A haunt packet has successfully reached thet target
        /// </summary>
        public void OnPacketSuccess(Hauntable target)
        {
            Debug.Log("Packet success!");
            if (target.CostIsFulfilled())
                SetPendingHauntable(target);
        }

        /// <summary>
        /// Sets the given target as the hauntable that will be haunted when haunt-targeting mode is exited.
        /// </summary>
        void SetPendingHauntable(Hauntable target)
        {
            _indicator = Instantiate(hauntIndicatorPrefab, target.transform.position, quaternion.identity, target.transform);
            _pendingHauntable = target;
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
            if (_pendingHauntable)
                BeginHaunt(_pendingHauntable);

            else 
                ghostState = GhostState.Normal;
            
            ClearTargetedHauntable();
            onHauntStateEnd.Invoke();
            
            _targetingModeTimer = targetingModeCooldown.Value;
        }

        void BeginHaunt(Hauntable newHaunted)
        {
            haunted = newHaunted;
            haunted.Posess();
            DestroyHauntPackets();
            ghostState = GhostState.Haunting;
            onHauntBegin.Invoke();
            _rigidbody.isKinematic = true;
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
            _rigidbody.isKinematic = false;
            haunted.OnUnHaunted();
            onHauntEnd.Invoke();
            haunted = targetedHauntable = null;
            RecallHauntPackets();
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
            savedHauntJuice.Save(hauntJuice);
            onHauntStateEnd.Invoke();
            hauntBurstIntensityRef.Value = 0;
        }

        public void AddJuice(int amount)
        {
            hauntJuice += amount;
            onJuiceAdded.Invoke();

            _hauntGuiTimed = true;
            _hauntGuiTimer = showTime;
            ShowJuiceGui();
        }

        void HideJuiceGui()
        {
            _hauntGuiTimed = false;
            if (_juiceGuiInstance)
                _juiceGuiInstance.Hide();
        }
        
        public void ShowJuiceGui()
        {
            if (!_juiceGuiInstance)
            {
                GameObject newHauntGui = Instantiate(hauntJuiceQtyGuiPrefab, transform.position + Vector3.up,
                    quaternion.identity);

                _juiceGuiInstance = newHauntGui.GetComponent<HauntJuiceGui>();
                _juiceGuiInstance.Init(this);
            }
            else _juiceGuiInstance.Show();
        }
    }
}