using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

[TypeInfoBox("Handles the extra events and stuff related to the player being damaged. This " +
             "needs to be combined with a Health component.")]
public class PlayerHealth : HealthPlugin
{
    public SavedInt savedPlayerHealth;
    public UnityEvent onHealthCritical;
    public UnityEvent onHealthOkay;

    public void SetSavedHealthValue()
    {
        if (savedPlayerHealth.HasSavedValue())
        {
            health.SetHp(savedPlayerHealth.GetValue());
            CheckForCritical();
        }
    }

    protected override void Damaged(int newHp)
    {
        base.Damaged(newHp);
        CheckForCritical();
    }

    protected override void Healed(int newHp)
    {
        base.Healed(newHp);
        if (newHp > 1)
            onHealthOkay.Invoke();
    }

    void CheckForCritical()
    {
        if (health.CurrentHealth < 2)
            onHealthCritical.Invoke();
    }

    void OnDestroy()
    {
        // Don't save the health value if the player was destroyed because HP reached 0
        if (health.IsKilled) return;
        savedPlayerHealth.Save(health.CurrentHealth);
    }
}
