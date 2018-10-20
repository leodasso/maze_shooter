using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public class Ship : MonoBehaviour
{
	public Vector2 movementInput;
	public FloatReference enginePower;

	Rigidbody2D _rigidbody2D;

	// Use this for initialization
	void Start ()
	{
		_rigidbody2D = GetComponent<Rigidbody2D>();
		if (!_rigidbody2D)
		{
			Debug.LogError("No rigidbody2D component could be found on " + name + ", ending ship component.");
			enabled = false;
			return;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		Vector2 clampedMoveInput = Vector2.ClampMagnitude(movementInput, 1);
		_rigidbody2D.AddForce(clampedMoveInput * enginePower.Value * Time.fixedDeltaTime);
	}
}
