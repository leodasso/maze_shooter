using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

[AddComponentMenu("Effects/Effect Spawners/On Enable Effect")]
public class OnEnableEffect : EffectsBase
{
	void OnEnable()
	{
		InstantiateEffect();
	}
}
