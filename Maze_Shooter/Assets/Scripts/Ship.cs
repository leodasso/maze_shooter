using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public class Ship : MonoBehaviour, IControllable
{
	public FloatReference enginePower;
	Rigidbody _rigidbody;
	Vector2 _moveVector;

	// Use this for initialization
	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody>();
		if (!_rigidbody)
		{
			Debug.LogError("No rigidbody component could be found on " + name + ", ending ship component.");
			enabled = false;
		}
	}

	void FixedUpdate()
	{
		Vector2 clampedMoveInput = Vector2.ClampMagnitude(_moveVector, 1);
		Vector3 force3D = new Vector3(clampedMoveInput.x, 0, clampedMoveInput.y);
		_rigidbody.AddForce(force3D * enginePower.Value * Time.fixedDeltaTime);
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
