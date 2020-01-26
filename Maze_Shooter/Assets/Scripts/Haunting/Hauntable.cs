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
        
        HauntCostGui _hauntCostGuiInstance;
        List<HauntPacket> _hauntPackets = new List<HauntPacket>();
        
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

        [Button]
        public void Posess()
        {
            Debug.Log(name + " was haunted!", gameObject);
            onHaunted.Invoke();
        }

        public void OnUnHaunted()
        {
            onUnHaunted.Invoke();
        }
    }
}