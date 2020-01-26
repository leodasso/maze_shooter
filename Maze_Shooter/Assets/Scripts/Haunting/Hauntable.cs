using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace ShootyGhost
{
    [TypeInfoBox("Can be haunted by Haunter!")]
    public class Hauntable : MonoBehaviour
    {
        public int hauntCost = 5;
        public UnityEvent onHaunted;
        public UnityEvent onUnHaunted;
        public GameObject hauntCostGuiPrefab;
        public GameObject hauntedEffectPrefab;
        
        HauntCostGui _hauntCostGuiInstance;
        List<HauntPacket> _hauntPackets = new List<HauntPacket>();
        GameObject _hauntedEffectInstance;
        
        public int DisplayedHauntJuice => hauntCost - _hauntPackets.Count;

        void Start()
        {
            _hauntCostGuiInstance = Instantiate(hauntCostGuiPrefab).GetComponent<HauntCostGui>();
            _hauntCostGuiInstance.Init(this);
        }

        void OnDestroy()
        {
            if (_hauntCostGuiInstance)
                Destroy(_hauntCostGuiInstance);
        }

        public bool CostIsFulfilled()
        {
            return _hauntPackets.Count >= hauntCost;
        }

        public void AddHauntPacket(HauntPacket newPacket)
        {
            if (_hauntPackets.Contains(newPacket)) return;
            _hauntPackets.Add(newPacket);
        }

        public void LoseHauntPacket(HauntPacket packet)
        {
            _hauntPackets.Remove(packet);
        }

        public void TargetedForHaunt()
        {
            if (_hauntCostGuiInstance) 
                _hauntCostGuiInstance.ShowFull();
        }

        public void UnTargetedForHaunt()
        {
            if (_hauntCostGuiInstance)
                _hauntCostGuiInstance.Show();
        }

        /// <summary>
        /// Calculates and returns the position that the ghost should go to once exiting after haunt
        /// </summary>
        public Vector3 GetReturnPosition()
        {
            // TODO lol this prob needs better options
            return transform.position + Vector3.back * 3;
        }

        [Button]
        public void Posess()
        {
            _hauntPackets.Clear();
            onHaunted.Invoke();
            if (hauntedEffectPrefab)
            {
                _hauntedEffectInstance =
                    Instantiate(hauntedEffectPrefab, transform.position, Quaternion.identity, transform);
            }
        }

        public void OnUnHaunted()
        {
            if (_hauntedEffectInstance)
                Destroy(_hauntedEffectInstance);
            
            onUnHaunted.Invoke();
        }
    }
}