using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu]
public class GunData : ScriptableObject
{
	[Title("Fire Rate", "Min & Max")]
	[MinMaxSlider(.1f, 60, true), Tooltip("Number of shots per second. The minimum is when the player is barely touching joystick," +
	                                " and max is when they're at full tilt.")]
	public Vector2 firingRate;
	
	[Tooltip("Multiple firing patterns can be assigned so that this gun can level up")]
	public List<FiringPattern> firingPatterns = new List<FiringPattern>();

	[AssetsOnly, PreviewField, AssetList(AutoPopulate = false, Path = "Prefabs/Ammo")]
	public GameObject ammo;

	public int MaxLevel => Mathf.Max(0, firingPatterns.Count - 1);

}
