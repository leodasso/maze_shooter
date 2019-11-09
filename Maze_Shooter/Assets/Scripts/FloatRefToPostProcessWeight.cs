using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Arachnid;

/// <summary>
/// Applies the value of a float reference to a post-process volume's weight.
/// </summary>
[RequireComponent(typeof(PostProcessVolume))]
public class FloatRefToPostProcessWeight : MonoBehaviour
{
    bool applyOnUpdate = true;
    public FloatReference weightValue;
    public float lerpSpeed = 50;

    float _weight = 0;
    PostProcessVolume _volume;
    
    // Start is called before the first frame update
    void Start()
    {
        _volume = GetComponent<PostProcessVolume>();
        Apply();
    }

    // Update is called once per frame
    void Update()
    {
        if (applyOnUpdate)
            Apply();
    }

    void Apply()
    {
        _weight = Mathf.Lerp(_weight, weightValue.Value, Time.unscaledDeltaTime * lerpSpeed);
        if (_weight < .025f) _weight = 0;

        _volume.weight = _weight;
    }
}
