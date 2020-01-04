using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[TypeInfoBox("Calls events from stuff that would normally do damage, like getting shot and hazards and stuff. Useful for like," +
             " shooting something to turn it on/off, for example.")]
public class EventFromDamage : MonoBehaviour, IDestructible
{
    public UnityEvent onAttemptedDamage;

    public void DoDamage(int amount, Vector3 pos, Vector3 dir)
    {
        onAttemptedDamage.Invoke();
    }

    public void Destruct()
    {
    }
}
