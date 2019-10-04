using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Health)), RequireComponent(typeof(Flasher))]
public class InvulnerableFlash : MonoBehaviour
{
	Flasher _flasher;
	Health _health;

	// Use this for initialization
	void Start ()
	{
		_health = GetComponent<Health>();
		_flasher = GetComponent<Flasher>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!_health || !_flasher) return;
		_flasher.SetFlashing(_health.IsInvulnerable);
	}
}