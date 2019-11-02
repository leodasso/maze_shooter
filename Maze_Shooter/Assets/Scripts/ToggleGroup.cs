using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace ShootyGhost
{
    public enum ToggleGroupState
    {
        Mixed = 0, AllOff = 1, AllOn = 2
    }
    
    public class ToggleGroup : MonoBehaviour
    {
        public List<Toggle> toggles = new List<Toggle>();
        public UnityEvent onAllTogglesOn;
        public UnityEvent onAllTogglesOff;
        ToggleGroupState _groupState = ToggleGroupState.Mixed;
        ToggleGroupState _prevGroupState = ToggleGroupState.Mixed;

        [Button]
        public void GetTogglesInChildren()
        {
            toggles.Clear();
            toggles.AddRange(transform.GetComponentsInChildren<Toggle>());
        }

        // Start is called before the first frame update
        void Start()
        {
            _groupState = _prevGroupState = GetState();
        }

        // Update is called once per frame
        void Update()
        {
            _groupState = GetState();

            if (_groupState != _prevGroupState)
            {
                if (_groupState == ToggleGroupState.AllOn)
                    onAllTogglesOn.Invoke();

                if (_groupState == ToggleGroupState.AllOff)
                    onAllTogglesOff.Invoke();
            }

            _prevGroupState = _groupState;
        }

        
        /// <summary>
        /// Returns the current state of the toggles in this group, like are they all on, all off, or mixed
        /// </summary>
        ToggleGroupState GetState()
        {
            int off = 0;
            int on = 0;
            foreach (var toggle in toggles)
            {
                if (!toggle.gameObject.activeInHierarchy) continue;
                if (toggle.isOn) on++;
                else off++;
            }

            if (on == toggles.Count) return ToggleGroupState.AllOn;
            if (off == toggles.Count) return ToggleGroupState.AllOff;
            return ToggleGroupState.Mixed;
        }
    }
}