using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Rewired;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour
{
	[ReadOnly]
	public Vector2 moveInput;
	[ReadOnly]
	public Vector2 fireInput;
	
	public List<IControllable> controllables = new List<IControllable>();
	Rewired.Player _player;

	void GetAllControllables()
	{
		controllables.Clear();
		controllables.AddRange(GetComponentsInChildren<IControllable>());
	}

	// Use this for initialization
	void Start ()
	{
		_player = ReInput.players.GetPlayer(0);
	}

	void OnEnable()
	{
		GetAllControllables();
	}

	// Update is called once per frame
	void Update () 
	{
		// Don't take action if we're paused
		if (Time.timeScale <= Mathf.Epsilon) return;
		if (_player == null) return;
		
		// Get the player's inputs
		moveInput = new Vector2(_player.GetAxis("moveX"), _player.GetAxis("moveY"));
		fireInput = new Vector2(_player.GetAxis("fireX"), _player.GetAxis("fireY"));

		// tell the ship how to move based on player's input
		foreach (var controllable in controllables)
		{
			controllable.ApplyLeftStickInput(moveInput);
			controllable.ApplyRightStickInput(fireInput);
		}
	}
}