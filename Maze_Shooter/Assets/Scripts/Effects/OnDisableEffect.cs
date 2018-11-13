using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDisableEffect : EffectsBase 
{
	void OnDisable()
	{
		if (_lifetime > .1f)
			InstantiateEffect();
	}
}
