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
    public SavedInt savedPlayerHealth;
	public SavedInt savedPlayerMaxHealth;

	[SerializeField, Space]
    UnityEvent onHealthCritical;
	[SerializeField]
    UnityEvent onHealthOkay;

	protected override void Start()
	{
		base.Start();
		ApplyMaxHp();
		CheckForCritical();
	}

	void ApplyMaxHp() 
	{
		health.maxHearts.Value = savedPlayerMaxHealth.GetValue();
	}

    public void ApplySavedHp()
    {
        if (savedPlayerHealth.HasSavedValue())
        {
			Hearts newStartHearts = new Hearts();
			newStartHearts.SetTotalPoints(savedPlayerHealth.GetValue());
			newStartHearts = Hearts.Clamp(newStartHearts, minStartHp.Value, 100);
            health.SetHp(newStartHearts);
            CheckForCritical();
        }
    }

    public void CheckForCritical()
    {
        if (health.currentHp.Value < criticalHealth.Value)
            onHealthCritical.Invoke();
		else onHealthOkay.Invoke();
    }

    void OnDestroy()
    {
        savedPlayerHealth.Save(health.currentHp.Value.TotalPoints);
    }
}
