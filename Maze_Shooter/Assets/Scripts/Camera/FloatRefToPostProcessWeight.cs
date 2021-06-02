using UnityEngine;
using Arachnid;
using Sirenix.OdinInspector;

/// <summary>
/// Applies the value of a float reference to a post-process volume's weight.
/// </summary>
public class FloatRefToPostProcessWeight : PostProcessBase
{
    public bool applyOnUpdate = true;
    public bool useCurve;
    public FloatReference weightValue;
    
    [Tooltip("X axis is the weight value, and Y axis is the output to post processing weight")]
    [ShowIf("useCurve")]
    public AnimationCurve outputCurve;
    public float lerpSpeed = 50;

	float lerpedWeight;

    // Start is called before the first frame update
    void Start()
    {
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
        lerpedWeight = Mathf.Lerp(lerpedWeight, weightValue.Value, Time.unscaledDeltaTime * lerpSpeed);
        weight = useCurve ? outputCurve.Evaluate(lerpedWeight) : weight;
        
        // Prevent weird buggy stuff with post processing 
        if (weight < .001f)
            weight = 0;
    }
}
