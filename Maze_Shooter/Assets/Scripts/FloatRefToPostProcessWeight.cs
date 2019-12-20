using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Arachnid;
using Sirenix.OdinInspector;

/// <summary>
/// Applies the value of a float reference to a post-process volume's weight.
/// </summary>
[RequireComponent(typeof(PostProcessVolume))]
public class FloatRefToPostProcessWeight : MonoBehaviour
{
    public bool applyOnUpdate = true;
    public bool useCurve;
    public FloatReference weightValue;
    
    [Tooltip("X axis is the weight value, and Y axis is the output to post processing weight")]
    [ShowIf("useCurve")]
    public AnimationCurve outputCurve;
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
        _volume.weight = useCurve ? outputCurve.Evaluate(_weight) : _weight;
        
        // Prevent weird buggy stuff with post processing 
        if (_volume.weight < .001f)
            _volume.weight = 0;
    }
}
