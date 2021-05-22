using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Playmaker and some others can't directly interface with particle systems,
/// so there's this!
/// </summary>
public class ParticleHelper : MonoBehaviour
{
	public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

	[ButtonGroup]
	public void Play() 
	{
		foreach (var p in particleSystems) p.Play();
	}

	[ButtonGroup]
	public void Stop() 
	{
		foreach (var p in particleSystems) p.Stop();
	}
}
