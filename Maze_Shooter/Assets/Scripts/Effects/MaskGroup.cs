using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MaskGroup : MonoBehaviour
{
    public List<SpriteRenderer> renderers = new List<SpriteRenderer>();
	public List<ParticleSystemRenderer> particles = new List<ParticleSystemRenderer>();
    [OnValueChanged("SetMaskInteraction")]
    public SpriteMaskInteraction maskInteraction = SpriteMaskInteraction.None;
	
	void Start() {
		SetMaskInteraction();
	}
    
    [Button]
    void GetRenderers()
    {
        renderers.Clear();
        renderers.AddRange(GetComponentsInChildren<SpriteRenderer>());

		particles.Clear();
		particles.AddRange(GetComponentsInChildren<ParticleSystemRenderer>());
    }

    void SetMaskInteraction()
    {
        foreach (SpriteRenderer r in renderers)
            r.maskInteraction = maskInteraction;

		foreach (ParticleSystemRenderer p in particles) {
			p.maskInteraction = maskInteraction;
		}
    }

    public void SetMaskInteraction(SpriteMaskInteraction newMaskInteraction)
    {
        maskInteraction = newMaskInteraction;
        SetMaskInteraction();
    }

    public void SetAsMasked()
    {
        maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        SetMaskInteraction();
    }
}