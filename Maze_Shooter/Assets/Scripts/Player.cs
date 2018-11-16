using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;
using Rewired;
using Sirenix.OdinInspector;

public class Player : MonoBehaviour
{
	public GameObject gunRotator;
	public Gun gun;

	[ReadOnly]
	public Vector2 moveInput;
	public FloatReference fireThreshhold;
	public SpriteRotation spriteRotation;
	bool _firing;
	Rewired.Player _player;
	Ship _ship;

	// Use this for initialization
	void Start ()
	{
		_ship = GetComponent<Ship>();
		_player = ReInput.players.GetPlayer(0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Don't take action if we're paused
		if (Time.timeScale <= Mathf.Epsilon) return;
		
		// Get the player's inputs
		moveInput = new Vector2(_player.GetAxis("moveX"), _player.GetAxis("moveY"));
		Vector2 fireInput = new Vector2(_player.GetAxis("fireX"), _player.GetAxis("fireY"));

		// check if the player is firing
		_firing = fireInput.magnitude >= fireThreshhold.Value;

		// tell the ship how to move based on player's input
		_ship.movementInput = moveInput;
		
		gun.firing = _firing;
		gun.fireRateIntensity = fireInput.magnitude;
		
		if (!_firing) return;
		
		// Tell the gun where to fire
		float angle =  Math.AngleFromVector2(new Vector2(fireInput.x, fireInput.y), -90);
		if (spriteRotation) spriteRotation.Rotation = angle;
		gunRotator.transform.localEulerAngles = new Vector3(0, 0, angle);
	}
}
