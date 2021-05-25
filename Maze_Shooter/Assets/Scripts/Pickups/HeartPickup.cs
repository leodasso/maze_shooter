using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class HeartPickup : Pickup
{
	[SerializeField]
	UnityEvent onBreakApart;

	[PropertyOrder(-200)]
	public Hearts amount = 1;

	public override void GetGulped()
	{
		var col = GetComponent<Collider>();
		if (col)
			col.enabled = false;

		if (autoDestroy)
        	Destroy(gameObject, destroyDelay);
	}

	public void BreakApart()
	{
		onBreakApart.Invoke();
	}

	public void AddHearts() 
	{
		onGrabbed.Invoke();
	}

}