using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[TypeInfoBox("Handles the extra events and stuff related to the player being damaged. This " +
             "needs to be combined with a Health component.")]
public class PlayerHealth : HealthPlugin
{
    public UnityEvent onHealthCritical;
    public UnityEvent onHealthOkay;

    protected override void Damaged(int newHp)
    {
        base.Damaged(newHp);
        if (newHp < 2)
            onHealthCritical.Invoke();
    }

    protected override void Healed(int newHp)
    {
        base.Healed(newHp);
        if (newHp > 1)
            onHealthOkay.Invoke();
    }
}
