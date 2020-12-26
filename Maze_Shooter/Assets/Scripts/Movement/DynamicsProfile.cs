using UnityEngine;

[ CreateAssetMenu(menuName = "Ghost/Dynamics profile")]
public class DynamicsProfile : ScriptableObject
{
	public float drag = 15;
	public float maxSpeed = 10;
	public float acceleration = 5;
	public float rigidbodyMass = 5;
	public float rigidbodyDrag = 0;
	public bool rigidbodyGravity = true;
}