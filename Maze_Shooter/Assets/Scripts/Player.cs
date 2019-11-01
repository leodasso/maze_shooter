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
	
	[LabelText("gun graphic"), Tooltip("Optional - link a sprite rotator component to show the rotation of the gun")]
	public SpriteRotation spriteRotation;

	[Tooltip("Optional - animator to send speed and movement angle to")]
	public Animator animator;
	[Tooltip("This doesn't affect movement! Just offsets the movement angle that gets sent to the animator."), ShowIf("hasAnimator")]
	public float movementAngleOffset = 45;
	 
	bool _firing;
	Rewired.Player _player;
	Ship _ship;
	Rigidbody2D _rigidbody2D;

	bool hasAnimator => animator != null;

	// Use this for initialization
	void Start ()
	{
		_ship = GetComponent<Ship>();
		_rigidbody2D = GetComponent<Rigidbody2D>();
		_player = ReInput.players.GetPlayer(0);
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Don't take action if we're paused
		if (Time.timeScale <= Mathf.Epsilon) return;

		if (_player == null) return;
		
		// Get the player's inputs
		moveInput = new Vector2(_player.GetAxis("moveX"), _player.GetAxis("moveY"));
		Vector2 fireInput = new Vector2(_player.GetAxis("fireX"), _player.GetAxis("fireY"));

		// check if the player is firing
		_firing = fireInput.magnitude >= fireThreshhold.Value;

		// tell the ship how to move based on player's input
		_ship.movementInput = moveInput;
		
		gun.firing = _firing;
		gun.fireRateIntensity = fireInput.magnitude;

		// Send parameters to animator based on movement
		if (animator)
		{
			if (_rigidbody2D)
				animator.SetFloat("speed", _rigidbody2D.velocity.magnitude);
			
			animator.SetFloat("facingAngle", Math.AngleFromVector2(moveInput, movementAngleOffset));
		}
		
		if (!_firing) return;
		
		// Tell the gun where to fire
		float angle =  Math.AngleFromVector2(new Vector2(fireInput.x, fireInput.y), -90);
		if (spriteRotation) spriteRotation.Rotation = angle;
		gunRotator.transform.localEulerAngles = new Vector3(0, 0, angle);
	}
}
