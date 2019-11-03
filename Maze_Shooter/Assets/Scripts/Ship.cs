using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public class Ship : MonoBehaviour, IControllable
{
	public FloatReference enginePower;
	Rigidbody2D _rigidbody2D;
	Vector2 _moveVector;

	// Use this for initialization
	void Start ()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
		if (!_rigidbody2D)
		{
			Debug.LogError("No rigidbody2D component could be found on " + name + ", ending ship component.");
			enabled = false;
		}
	}

	void FixedUpdate()
	{
		Vector2 clampedMoveInput = Vector2.ClampMagnitude(_moveVector, 1);
		_rigidbody2D.AddForce(clampedMoveInput * enginePower.Value * Time.fixedDeltaTime);
	}

	public void ApplyLeftStickInput(Vector2 input)
	{
		_moveVector = input;
	}

	public void ApplyRightStickInput(Vector2 input)
	{ }

	public string Name()
	{
		return "ship: " + name;
	}
}
