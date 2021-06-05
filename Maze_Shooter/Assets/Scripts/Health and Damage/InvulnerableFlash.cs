using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Sirenix.OdinInspector;

[TypeInfoBox("When the referenced health is invulnerable, flashes the renderers using Flasher component.")]
[RequireComponent(typeof(Flasher))]
public class InvulnerableFlash : MonoBehaviour
{
	Flasher _flasher;

	[SerializeField]
	Health _health;

	// Use this for initialization
	void Start ()
	{
		if (_health == null)
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