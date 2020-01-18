using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Sirenix.OdinInspector;

[RequireComponent(typeof(PostProcessVolume))]
public class PostProcessVolumeHelper : MonoBehaviour
{
    public float lerpSpeed = 10;
    [ReadOnly]
    public bool active;
    PostProcessVolume _volume;
    
    void Awake()
    {
        _volume = GetComponent<PostProcessVolume>();
    }

    /// <summary>
    /// Snaps the volume's weight to the given value without lerping
    /// </summary>
    public void SnapVolumeWeight(float snapTo)
    {
        _volume.weight = snapTo;
    }

    /// <summary>
    /// Adds the given amount to the volume's weight without lerping
    /// </summary>
    public void AddVolumeWeight(float amt)
    {
        _volume.weight += amt;
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
        float weight = active ? 1 : 0;
        _volume.weight = Mathf.Lerp(_volume.weight, weight, Time.unscaledDeltaTime * lerpSpeed);
    }
}
