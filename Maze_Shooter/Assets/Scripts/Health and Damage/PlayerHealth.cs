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

	[Space]
    public SavedHearts savedPlayerHealth;
	public SavedHearts savedPlayerMaxHealth;

	[SerializeField, Space]
    UnityEvent onHealthCritical;
	[SerializeField]
    UnityEvent onHealthOkay;

	protected override void Start()
	{
		base.Start();
		health.maxHearts.Value = savedPlayerMaxHealth.runtimeValue;
		CheckForCritical();
	}

    public void ApplySavedHp()
    {
        if (savedPlayerHealth.HasSavedValue())
        {
			Hearts newStartHearts = new Hearts();
			newStartHearts = savedPlayerHealth.runtimeValue;
			newStartHearts = Hearts.Clamp(newStartHearts, minStartHp.Value, 100);
            health.SetHp(newStartHearts);
            CheckForCritical(); 
        }

		else {
			// If there's no saved value, just set HP to max.
			health.SetHp(health.maxHearts.Value);
		}
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

    void OnDestroy()
    {
		savedPlayerHealth.runtimeValue = health.currentHp.Value;
        // savedPlayerHealth.Save(health.currentHp.Value.TotalPoints);
    }
}
