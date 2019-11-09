using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(PostProcessVolume))]
public class PostProcessVolumeHelper : MonoBehaviour
{
    public float lerpSpeed = 10;
    public bool active;
    PostProcessVolume _volume;
    
    void Awake()
    {
        _volume = GetComponent<PostProcessVolume>();
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
