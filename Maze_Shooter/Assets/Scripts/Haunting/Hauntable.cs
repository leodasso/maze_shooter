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

        GameObject _hauntCostGuiInstance;

        void Start()
        {
            _hauntCostGuiInstance = Instantiate(hauntCostGuiPrefab);
            _hauntCostGuiInstance.GetComponent<HauntCostGui>().Init(this);
        }

        void OnDestroy()
        {
            if (_hauntCostGuiInstance)
                Destroy(_hauntCostGuiInstance);
        }

        public void TargetedForHaunt()
        {
            
        }

        public void UnTargetedForHaunt()
        {
            
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