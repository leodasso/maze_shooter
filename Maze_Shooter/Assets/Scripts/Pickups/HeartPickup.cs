using UnityEngine;
using Sirenix.OdinInspector;

public class HeartPickup : Pickup
{
	[PropertyOrder(-200)]
	public Hearts amount = 1;
}