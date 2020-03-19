using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGroupVolume : Volume
{
    public ColorGroup colorGroup;
    
    public override void ApplyVolume(float newValue)
    {
        if (!colorGroup) return;
        colorGroup.alpha = volumeCurve.Evaluate(newValue);
    }
}
