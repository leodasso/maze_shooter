using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace ShootyGhost
{
    public class Toggle : MonoBehaviour
    {
        [ToggleLeft]
        public bool isOn;

        public UnityEvent onToggle;
        public UnityEvent onTurnOn;
        public UnityEvent onTurnOff;

        public void ToggleMe()
        {
            onToggle.Invoke();
            if (isOn) TurnOff();
            else TurnOn();
        }

        public void TurnOff()
        {
            if (!isOn) return;
            isOn = false;
            onTurnOff.Invoke();
        }

        public void TurnOn()
        {
            if (isOn) return;
            isOn = true;
            onTurnOn.Invoke();
        }
    }
}
