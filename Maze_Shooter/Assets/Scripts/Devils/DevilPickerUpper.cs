using System.Collections;
using UnityEngine;

public class DevilPickerUpper : MonoBehaviour
{
    public DevilLauncher launcher;
    
    void OnTriggerEnter(Collider other)
    {
        Devil devil = other.GetComponent<Devil>();
        if (!devil) return;
        if (devil.CanBePickedUp())
            devil.TouchedByLauncher(launcher);
    }
}
