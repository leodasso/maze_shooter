using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Effects/Effect Spawners/On Disable Effect")]
public class OnDisableEffect : EffectsBase
{
	[Tooltip("There's a minimum lifetime of 1/10th of a second. This prevents spawning effects of objects that were" +
	         " created and destroyed in the same frame. If you want to disable this for this object, here is how.")]
	public bool ignoreMinLifetime;
	
	void OnDisable()
	{
		if (!ignoreMinLifetime && _lifetime <= .1f) return;
		InstantiateEffect();
	}
}
