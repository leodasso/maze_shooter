using UnityEngine;

[ CreateAssetMenu(menuName = "Ghost/Dynamics profile")]
public class DynamicsProfile : ScriptableObject
{
	public float drag = 15;
	public float maxSpeed = 10;
	public float acceleration = 5;
	public bool rigidbodyGravity = true;
}