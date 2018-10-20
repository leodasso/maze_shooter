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
		moveInput = new Vector2(_player.GetAxis("moveX"), _player.GetAxis("moveY"));
		Vector2 fireInput = new Vector2(_player.GetAxis("fireX"), _player.GetAxis("fireY"));

		_firing = fireInput.magnitude >= fireThreshhold.Value;

		_ship.movementInput = moveInput;
		if (_firing)
		{
			float angle =  Arachnid.Math.AngleFromVector2(new Vector2(fireInput.x, fireInput.y), -90);
			gunRotator.transform.localEulerAngles = new Vector3(0, 0, angle);
		}

		gun.firing = _firing;
	}
}
