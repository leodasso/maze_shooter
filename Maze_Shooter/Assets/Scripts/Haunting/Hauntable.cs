using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace ShootyGhost
{
    [TypeInfoBox("Can be haunted by Haunter!")]
    public class Hauntable : MonoBehaviour
    {
        public UnityEvent onHaunted;
        public UnityEvent onUnHaunted;


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