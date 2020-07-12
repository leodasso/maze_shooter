using UnityEngine;

public class Barrier : MonoBehaviour
{
    [Tooltip("Renderer to preview the barrier in editor. Gets disabled on awake.")]
    public Renderer previewRenderer;

    public new ParticleSystem particleSystem;

    [Tooltip("The particle system will adjust emission based on the x scale of this. How many particles should" +
             " be emitted per 1 scale unit?")]
    public float emissionPerUnit = 20;
    
    // Start is called before the first frame update
    void Awake()
    {
        previewRenderer.enabled = false;
        var particleEmissionRate = particleSystem.emission.rateOverTime;
        particleEmissionRate.constant = emissionPerUnit * transform.localScale.x;
    }

}
