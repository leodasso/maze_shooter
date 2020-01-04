using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class MaskGroup : MonoBehaviour
{
    public List<SpriteRenderer> renderers = new List<SpriteRenderer>();
    [OnValueChanged("SetMaskInteraction")]
    public SpriteMaskInteraction maskInteraction = SpriteMaskInteraction.None;
    
    [Button]
    void GetRenderers()
    {
        renderers.Clear();
        renderers.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }

    void SetMaskInteraction()
    {
        foreach (SpriteRenderer r in renderers)
        {
            r.maskInteraction = maskInteraction;
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