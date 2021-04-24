using Sirenix.OdinInspector;
using UnityEngine;

[TypeInfoBox("Controls the alpha of a selected color group based on a shadowObject's " +
             "distance from its caster.")]
			  
[AddComponentMenu("Shadows/Shadow Plugin Alpha")]
public class ShadowObjectPluginAlpha : ShadowObjectPlugin
{
    public ColorGroup colorGroup;
    public AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);


    public override void Recalculate(float normalizedDist)
    {
        colorGroup.alpha = alphaCurve.Evaluate(normalizedDist);
    }
}