using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ CreateAssetMenu(menuName = "Ghost/Dynamics profile")]
public class DynamicsProfile : ScriptableObject
{
	public float rigidbodyMass = 1;
	public float rigidbodyDrag = 10;
	public bool rigidbodyGravity = true;
	public float movementForce = 10;
}
