using System.Collections;
using System.Collections.Generic;
using Arachnid;
using UnityEngine;

public class ShipObjectRotator : MonoBehaviour
{
	public Player player;
	public FloatReference inputThreshhold;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!player) return;
		
		if (player.moveInput.magnitude > inputThreshhold.Value)
			transform.localEulerAngles = new Vector3(0, 0, Math.AngleFromVector2(player.moveInput, -90));
	}
}
