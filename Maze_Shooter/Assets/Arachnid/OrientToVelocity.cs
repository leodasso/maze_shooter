using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arachnid {

	[ExecuteAlways]
	public class OrientToVelocity : MonoBehaviour
	{
		public PseudoVelocity pseudoVelocity;
		public float degreesOffset;

		// Update is called once per frame
		void Update()
		{
			if (!pseudoVelocity) return;
			if (pseudoVelocity.velocity.magnitude < .01f) return;
			float angle = Math.AngleFromVector2(pseudoVelocity.velocity, degreesOffset);

			transform.localEulerAngles = new Vector3(0, 0, angle);
		}
	}
}