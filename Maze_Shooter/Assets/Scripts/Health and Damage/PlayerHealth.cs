﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Arachnid;

[TypeInfoBox("Handles the extra events and stuff related to the player being damaged. This " +
             "needs to be combined with a Health component.")]
public class PlayerHealth : HealthPlugin
{
	public HeartsRef minStartHp;
    public SavedInt savedPlayerHealth;
	public SavedInt savedPlayerMaxHealth;
    public UnityEvent onHealthCritical;
    public UnityEvent onHealthOkay;

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
        if (health.currentHp.Value.hearts < 2)
            onHealthCritical.Invoke();
		else onHealthOkay.Invoke();
    }

    void OnDestroy()
    {
        savedPlayerHealth.Save(health.currentHp.Value.TotalPoints);
    }
}
