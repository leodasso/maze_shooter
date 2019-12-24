using ShootyGhost;
using UnityEngine;
using UnityEngine.Events;

public class HauntGlob : MonoBehaviour
{
    public int value = 1;
    public UnityEvent onGrabbed;

    public bool _used;
    

    void OnTriggerEnter(Collider other)
    {
        Haunter haunter = other.GetComponent<Haunter>();
        if (!haunter) return;
        
        if (_used) return;
        _used = true;
        onGrabbed.Invoke();
        haunter.AddJuice(value);
    }
}
