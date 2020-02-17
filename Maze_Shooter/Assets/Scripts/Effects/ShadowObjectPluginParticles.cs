using Sirenix.OdinInspector;
using UnityEngine;

[TypeInfoBox("Controls particle emmission based on a given shadow object's distance " +
             "from its caster.")]
public class ShadowObjectPluginParticles : ShadowObjectPlugin
{
    public new ParticleSystem particleSystem;
    public AnimationCurve emissionCurve = AnimationCurve.EaseInOut(0, 0, 1, 5);
    
    public override void Recalculate(float normalizedDist)
    {
        var emissionModule = particleSystem.emission;
        emissionModule.rateOverTimeMultiplier = emissionCurve.Evaluate(normalizedDist);
    }
}
