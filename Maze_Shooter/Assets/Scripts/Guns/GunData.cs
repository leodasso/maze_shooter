using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Ghost/Gun Data")]
public class GunData : ScriptableObject
{
	[Title("Fire Rate", "Min & Max")]
	[MinMaxSlider(.1f, 60, true), Tooltip("Number of shots per second. The minimum is when the player is barely touching joystick," +
	                                " and max is when they're at full tilt.")]
	public Vector2 firingRate;

	[Tooltip("spreads the bullets randomly. Higher value means less accurate. (euler angle)"), MinValue(0)]
	public float randomSpread;

	[AssetsOnly, PreviewField, AssetList(AutoPopulate = false, Path = "Prefabs/Ammo")]
	public GameObject ammo;
}
