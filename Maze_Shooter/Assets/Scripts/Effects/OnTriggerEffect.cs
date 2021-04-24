using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[AddComponentMenu("Effects/Effect Spawners/On Trigger Effect")]
public class OnTriggerEffect : EffectsBase 
{
    [Button]
    public void InvokeEffect()
    {
        InstantiateEffect();
    }
}
