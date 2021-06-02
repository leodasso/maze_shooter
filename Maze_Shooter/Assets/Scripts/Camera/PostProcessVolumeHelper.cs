using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using Sirenix.OdinInspector;

public class PostProcessVolumeHelper : PostProcessBase
{
    public float lerpSpeed = 10;
    [ReadOnly]
    public bool active;
    

    /// <summary>
    /// Snaps the volume's weight to the given value without lerping
    /// </summary>
    public void SnapVolumeWeight(float snapTo)
    {
        weight = snapTo;
    }

    /// <summary>
    /// Adds the given amount to the volume's weight without lerping
    /// </summary>
    public void AddVolumeWeight(float amt)
    {
        weight += amt;
    }

    // Active and inactive functions are for UnityEvent to ref
    public void SetActive()
    {
        active = true;

    }

    public void SetInactive()
    {
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        float newWeight = active ? 1 : 0;
        weight = Mathf.Lerp(weight, newWeight, Time.unscaledDeltaTime * lerpSpeed);
    }
}
