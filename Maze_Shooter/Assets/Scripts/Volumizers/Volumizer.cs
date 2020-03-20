using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Convert anything (kind of) into a normalized value, i.e. volumize it. This is a base class, but examples of
/// extensions would be 'representing distance to an object as a value between 0 and 1'
/// </summary>
public class Volumizer : MonoBehaviour
{
    [Tooltip("Clamps between 0 and 1")]
    public bool clampValue;
    public float normalizedValue;
    public List<Volume> volumes = new List<Volume>();

    protected virtual void UpdateVolumes()
    {
        foreach (Volume volume in volumes)
        {
            volume.ApplyVolume(normalizedValue);
        }
    }

    protected virtual void UpdateNormalizedValue(float newValue)
    {
        if (clampValue)
            newValue = Mathf.Clamp01(newValue);

        normalizedValue = newValue;
    }

    [Button]
    void GetAllVolumes()
    {
        volumes.Clear();
        volumes.AddRange(GetComponentsInChildren<Volume>());
    }

}
