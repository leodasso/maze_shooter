using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Arachnid;

[TypeInfoBox("Handles the extra events and stuff related to the player being damaged. This " +
             "needs to be combined with a Health component.")]
public class PlayerHealth : HealthPlugin
{
	[SerializeField]
	HeartsRef minStartHp;
	[SerializeField]
	HeartsRef criticalHealth;

	[SerializeField, Space]
    UnityEvent onHealthCritical;
	[SerializeField]
    UnityEvent onHealthOkay;

	protected override void Start()
	{
		base.Start();
		CheckForCritical();
	}

    public void ApplySavedHp()
    {
		health.currentHp.Value = Hearts.Clamp(health.currentHp.Value, minStartHp.Value, 100);
    }

	public void ProcessHpChange()
	{
		CheckForCritical();
	}

    void CheckForCritical()
    {
        if (health.currentHp.Value < criticalHealth.Value)
            onHealthCritical.Invoke();
		else onHealthOkay.Invoke();
    }
}