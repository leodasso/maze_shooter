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
	public IntReference minStartHp;
    public SavedInt savedPlayerHealth;
    public UnityEvent onHealthCritical;
    public UnityEvent onHealthOkay;

	protected override void Start()
	{
		base.Start();
		CheckForCritical();
	}


    public void ApplySavedHp()
    {
        if (savedPlayerHealth.HasSavedValue())
        {
			int savedHp = savedPlayerHealth.GetValue();
			savedHp = Mathf.Clamp(savedHp, minStartHp.Value, 100);
            health.SetHp(savedHp);
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
        if (health.currentHp.Value < 2)
            onHealthCritical.Invoke();
		else onHealthOkay.Invoke();
    }

    void OnDestroy()
    {
        savedPlayerHealth.Save(health.currentHp.Value);
    }
}
