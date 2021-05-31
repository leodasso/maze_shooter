using UnityEngine;
using UnityEngine.Events;
using Arachnid;
using Sirenix.OdinInspector;

public abstract class MovementMod : MonoBehaviour
{
	[SerializeField]
	MovementBase movementBase;

	protected Vector3 direction => movementBase.GetDirection();
	protected Vector3 lastDirection => movementBase.GetLastDirection();

	void OnEnable()
	{
		movementBase.AddMod(this);
	}

	void OnDisable()
	{
		movementBase.RemoveMod(this);
	}

	public abstract Vector3 ModifyVelocity(Vector3 input);

	public abstract void DoActionAlpha();
}