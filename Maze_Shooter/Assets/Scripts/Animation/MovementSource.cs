using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace ShootyGhost {

	[System.Serializable]
	public class MovementSource
	{

		[HideLabel, SerializeField]
		DirectionSourceType source = DirectionSourceType.Rigidbody;

		[ShowIf("SourceIsCustom"), HideLabel, SerializeField]
		public Vector3 customDirection;

		[ShowIf("SourceIsRigidbody"), HideLabel, SerializeField]
		new Rigidbody rigidbody;

		[ShowIf("SourceIsMover"), HideLabel, SerializeField]
		MovementBase mover;

		[ShowIf("SourceIsPseudo"), HideLabel, SerializeField]
		PseudoVelocity pseudoVelocity;

		// These are so odin knows which properties to show
		bool SourceIsRigidbody => source == DirectionSourceType.Rigidbody;
		bool SourceIsMover => source == DirectionSourceType.Mover;
		bool SourceIsPseudo => source == DirectionSourceType.PseudoVelocity;
		bool SourceIsCustom => source == DirectionSourceType.Custom;

		public Vector3 GetMovementVector()
		{
			switch (source)
			{
				case DirectionSourceType.Custom: return customDirection; 
				case DirectionSourceType.Mover: return mover.GetDirection();
				case DirectionSourceType.Rigidbody: return rigidbody.velocity;
				case DirectionSourceType.PseudoVelocity: return pseudoVelocity.velocity;
				default: return Vector3.zero;
			}
		}
	}
}