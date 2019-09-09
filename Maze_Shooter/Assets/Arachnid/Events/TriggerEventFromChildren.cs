using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Arachnid
{
    public class TriggerEventFromChildren : MonoBehaviour
    {
        [AssetsOnly]
        [BoxGroup("childChange", GroupName = "Child count changed")]
        [LabelText("Game Events")]
        public List<GameEvent> childChangeEvents = new List<GameEvent>();
        [BoxGroup("childChange")]
        [LabelText("Unity Event")]
        public UnityEvent childChangeUnityEvent;
        
        [AssetsOnly]
        [BoxGroup("noChildren", GroupName = "Last Child Destroyed")]
        [LabelText("Game Events")]
        public List<GameEvent> noChildrenEvents = new List<GameEvent>();
        [BoxGroup("noChildren")]
        [LabelText("Unity Event")]
        public UnityEvent noChildrenUnityEvents;

        int _childCount = 0;

        // Start is called before the first frame update
        void Start()
        {
            _childCount = transform.ActiveChildCount();
        }

        void Update()
        {
            int currentChildCount = transform.ActiveChildCount();
            if (_childCount != currentChildCount)
            {
                OnChildCountChanged();
                _childCount = currentChildCount;
                if (_childCount == 0)
                {
                    OnAllChildrenGone();
                }
            }
        }

        void OnChildCountChanged()
        {
            foreach (GameEvent e in childChangeEvents) e.Raise();
            childChangeUnityEvent.Invoke();
        }

        void OnAllChildrenGone()
        {
            Debug.Log("All children gone!");
            foreach (GameEvent e in noChildrenEvents) e.Raise();
            noChildrenUnityEvents.Invoke();
        }
    }
}