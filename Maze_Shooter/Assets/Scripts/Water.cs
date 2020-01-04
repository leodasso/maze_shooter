﻿using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public class Water : ContactBase
{
	public FloatReference wetness;

	protected override void OnCollisionAction(Collision collision, Collider otherCol)
	{
		IWettable wettable = otherCol.GetComponent<IWettable>();
		wettable?.AddWater(wetness.Value);
	}

	protected override void OnTriggerAction(Collider other)
	{
		IWettable wettable = other.GetComponent<IWettable>();
		wettable?.AddWater(wetness.Value);
	}
}