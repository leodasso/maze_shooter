using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Rewired;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour
{
	[ToggleLeft]
	public bool debug;

	[ToggleLeft]
	public bool controlledByPlayer;
	
	[ReadOnly]
	public Vector2 moveInput;
	[ReadOnly]
	public Vector2 fireInput;
	
	public List<IControllable> controllables = new List<IControllable>();
	Rewired.Player _player;

	bool _alphaAction;

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

	void OnDisable()
	{
		moveInput = Vector2.zero;
		fireInput = Vector2.zero;
		_alphaAction = false;
		UpdateControllables();
	}

	// Update is called once per frame
	void Update () 
	{
		// Don't take action if we're paused
		if (Time.timeScale <= Mathf.Epsilon) return;

		if (controlledByPlayer)
			ApplyPlayerInputs();
		
		UpdateControllables();
	}

	void ApplyPlayerInputs() 
	{
		if (_player == null) return;
		moveInput = new Vector2(_player.GetAxis("moveX"), _player.GetAxis("moveY"));
		fireInput = new Vector2(_player.GetAxis("fireX"), _player.GetAxis("fireY"));
		_alphaAction = _player.GetButtonDown("alpha");
	}

	void UpdateControllables()
	{
		foreach (var controllable in controllables)
		{
			if (debug)
				Debug.Log(name + " updating controllable " + controllable.Name());
			
			controllable.ApplyLeftStickInput(moveInput);
			controllable.ApplyRightStickInput(fireInput);
			
			if (_alphaAction)
				controllable.DoActionAlpha();
		}
	}

	public void EnablePlayerControl() {
		controlledByPlayer = true;
	}

	public void DisablePlayerControl() {
		controlledByPlayer = false;
	}
}